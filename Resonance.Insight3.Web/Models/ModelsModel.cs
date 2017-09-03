using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ViewModels.Models;

namespace Resonance.Insight3.Web.Models
{
    public class ModelsModel : ModelBase
    {
        public static ModelDataViewModel GetModel(int modelId)
        {
            var viewModel = new ModelDataViewModel
                {
                    ModelID = modelId
                };

            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var request = new GetModelRequest
                        {
                            ModelID = modelId,
                            User = FormsAuthenticationWrapper.User
                        };
                        GetModelResponse response = _certonaService.GetModel(request);

                        var columnGroupHeaders = new List<string>();


                        if (response.Errors != null && response.Errors.Count != 0)
                        {
                            viewModel.Errors = response.Errors.ToList();
                        }

                        if (response.Model != null)
                        {
                            ModelDTO mDTO = response.Model;
                            GetModelData(mDTO, ref viewModel.GridData, ref columnGroupHeaders);
                        }

                        viewModel.ColumnGroupHeaders = new string[columnGroupHeaders.Count];
                        viewModel.ColumnGroupHeaders = columnGroupHeaders.ToArray();
                        if (response.Model != null)
                        {
                            viewModel.CatalogName = response.Model.CatalogName;
                            viewModel.ModelName = response.Model.Name;
                        }
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
                
            }
            catch (Exception ex)
            {
                viewModel.Errors.Add(new ErrorResult
                {
                    Description = ex.Message
                });
            }

            return viewModel;
        }

        public static SegmentItemsGridViewModel GetSegmentItems(int modelID, int segmentID)
        {
            var viewModel = new SegmentItemsGridViewModel
                {
                    SegmentID = segmentID
                };

            try
            {
                using(_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        var request = new GetSegmentItemsRequest
                        {
                            ModelID = modelID,
                            SegmentID = segmentID,
                            User = (UserDTO)HttpContext.Current.Session["user"]
                        };
                        GetSegmentItemsResponse response = _certonaService.GetSegmentItems(request);

                        if (response.SegmentItems.Count > 0)
                        {
                            viewModel.AccountItemIDDisplay = response.SegmentItems[0].AccountItemIdFieldValue;
                            viewModel.DescriptionDisplay = response.SegmentItems[0].AccountDescriptionFieldValue;
                        }

                        if (response.Errors != null && response.Errors.Count != 0)
                        {
                            viewModel.Errors = response.Errors.ToList();
                        }

                        foreach (SegmentItemDTO segment in response.SegmentItems)
                        {
                            viewModel.SegmentItems.Add(new SegmentItemsViewModel
                            {
                                ItemID = segment.ItemID,
                                Name = segment.AccountDescriptionValue,
                                Score = segment.Score,
                                SegmentID = segment.SegmentID,
                                SKU = segment.AccountItemIDValue
                            });
                        }
                    }
                    catch (TimeoutException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                    catch (CommunicationException exception)
                    {
                        _certonaService.Abort();
                        throw;
                    }
                }
                
            }
            catch (Exception ex)
            {
                viewModel.Errors.Add(new ErrorResult
                {
                    Description = ex.Message
                });
            }

            return viewModel;
        }

        private static void GetModelData(ModelDTO mDTO, ref DataTable dataTable, ref List<String> columnGroupHeaders)
        {
            var modelsGridDataTable = new DataTable("ModelsGridData");

            // Add the SegmentID column since it is always present
            modelsGridDataTable.Columns.Add(new DataColumn
            {
                AllowDBNull = true,
                ColumnName = "ID",
                DataType = Type.GetType("System.Int32")
            });
            modelsGridDataTable.Columns.Add(new DataColumn
            {
                AllowDBNull = true,
                ColumnName = "Name",
                DataType = Type.GetType("System.String")
            });

            foreach (SegmentDTO segment in mDTO.Segmentation.Segments)
            {
                foreach (SegmentAttributeDataDTO segmentAttributeData in segment.AttributeData)
                {
                    string attributeName = segmentAttributeData.AttributeTypeName;
                    if (!columnGroupHeaders.Contains(attributeName))
                    {
                        columnGroupHeaders.Add(attributeName);
                    }

                    Type thisType = Type.GetType("system." + segmentAttributeData.AttributeType, false, true);

                    if (!modelsGridDataTable.Columns.Contains("Start_" + attributeName))
                    {
                        modelsGridDataTable.Columns.Add(new DataColumn
                        {
                            AllowDBNull = true,
                            ColumnName = "Start_" + attributeName,
                            DataType = thisType
                        });
                    }

                    if (!modelsGridDataTable.Columns.Contains("End_" + attributeName))
                    {
                        modelsGridDataTable.Columns.Add(new DataColumn
                        {
                            AllowDBNull = true,
                            ColumnName = "End_" + attributeName,
                            DataType = thisType
                        });
                    }
                }
            }

            foreach (SegmentDTO segment in mDTO.Segmentation.Segments)
            {
                DataRow row = modelsGridDataTable.NewRow();
                row["Name"] = segment.Name;
                row["ID"] = segment.ID;
                foreach (SegmentAttributeDataDTO segmentAttributeData in segment.AttributeData)
                {
                    string attributeName = segmentAttributeData.AttributeTypeName;
                    switch (segmentAttributeData.AttributeType.ToLowerInvariant())
                    {
                        case "double":
                            double val;
                            if (double.TryParse(segmentAttributeData.StartValue, out val))
                            {
                                row["Start_" + attributeName] = val;
                            }
                            if (double.TryParse(segmentAttributeData.EndValue, out val))
                            {
                                row["End_" + attributeName] = val;
                            }
                            break;
                    }
                }
                modelsGridDataTable.Rows.Add(row);
            }

            dataTable = modelsGridDataTable;
        }
    }
}