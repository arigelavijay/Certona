using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Resonance.Insight3.Web.CertonaService;
using Resonance.Insight3.Web.FormAuthentication;
using Resonance.Insight3.Web.ViewModels.Variant;
using Resonance.Insight3.Web.ViewModels.Rule;

namespace Resonance.Insight3.Web.Models
{
    public class RuleModel : ModelBase
    {
        public static int CreateRule(int variantID, string ruleName)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new CreateRuleRequest()
                        {
                            User = user,
                            ApplicationID = null,
                            Description = string.Empty,
                            Enabled = true,
                            Name = ruleName,
                            VariantID = variantID
                        };
                        var response = _certonaService.CreateRule(request);
                        return response.RuleID;
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
                throw;
            }
        }

        public static bool DeleteRule(int ruleID)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new DeleteRuleRequest()
                        {
                            User = user,
                            ApplicationID = null,
                            RuleID = ruleID
                        };
                        var response = _certonaService.DeleteRule(request);
                        return response.Success;
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
                throw;
            }
        }

        public static List<CatalogAttribute> GetCatalogMappingFields(int ruleID)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        List<CatalogAttribute> list = new List<CatalogAttribute>();
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetCatalogByRuleRequest()
                        {
                            User = user,
                            ApplicationID = null,
                            RuleId = ruleID
                        };
                        var response = _certonaService.GetCatalogByRule(request);

                        //if (response.Success && response.Details != null)
                        //{
                        //    foreach (var catalog in response.Details)
                        //    {
                        //        var catalogRequest = new GetCatalogMappingFieldsRequest()
                        //        {
                        //            User = user,
                        //            ApplicationID = null,
                        //            CatalogID = catalog.CatalogID
                        //        };


                        //        var catalogResponse = _certonaService.GetCatalogMappingFields(catalogRequest);

                        //        if (catalogResponse.Success)
                        //        {
                        //            list.Add(new CatalogAttribute()
                        //            {
                        //                CatalogMappingFields = catalogResponse.CatalogFields,
                        //                CatalogID = catalog.CatalogID,
                        //                CatalogName = catalog.CatalogName
                        //            });
                        //        }
                        //    }

                        //}

                        return list;
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
                throw;
            }
        }

        public static List<RuleOperatorDTO> GetRuleOperators(string attributeType)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetRuleOperatorsRequest()
                        {
                            User = user,
                            ApplicationID = null,
                            AttributeType = attributeType
                        };
                        var response = _certonaService.GetRuleOperators(request);
                        return response.RuleOperators;
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
                throw;
            }
        }

        public static ControlSet GetControlSet(string attributeType, string operatorType)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new GetControlSetRequest()
                        {
                            User = user,
                            ApplicationID = null,
                            AttributeType = attributeType,
                            OperatorType = operatorType
                        };
                        var response = _certonaService.GetControlSet(request);
                        return response.ControlSet;
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
                throw;
            }
        }

        public static int SaveExpression(ExpressionViewModel expression)
        {
            try
            {
                using (_certonaService = new CertonaServiceClient())
                {
                    try
                    {
                        UserDTO user = FormsAuthenticationWrapper.User;
                        var request = new SaveExpressionRequest()
                        {
                            User = user,
                            ApplicationID = null,
                            VariantID = expression.VariantID,
                            RuleID = expression.RuleID,
                            RuleName = expression.RuleName,
                            ExpressionID = expression.ExpressionID
                        };
                        request.RuleExpression = new List<ExpressionDTO>();
                        request.RuleExpression.Add(new ExpressionDTO()
                        {
                            ConditionID = expression.ConditionID, // init to 0 for NEW ADD
                            Attribute = expression.Attribute,
                            Operator = expression.Operator,
                            Value = expression.Value,
                            Context = expression.Context,
                            PlusOrMinus = expression.PlusOrMinus,
                            PercentOrUnits = expression.PercentOrUnits,
                            ExpressionType = ExpressionType.Filter
                        });
                        var response = _certonaService.SaveExpression(request);
                        return response.ExpressionID;
                        // return 400;
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
                throw;
            }
        }
    }
}