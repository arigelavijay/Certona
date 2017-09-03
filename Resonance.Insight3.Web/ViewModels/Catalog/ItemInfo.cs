using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Resonance.Insight3.Web.CertonaService;

namespace Resonance.Insight3.Web.ViewModels.Catalog
{
    public class ItemInfo
    {
        public ItemInfo(CatalogItemInfoDTO dto)
        {
            ItemID = dto.ItemID;
            AccountID = dto.AccountID;
            AccountItemID = dto.AccountItemID;
            ObjectID = dto.ObjectID;
            CatalogID = dto.CatalogID;
            Disabled = dto.Disabled;
            DateIntroduced = dto.DateIntroduced;
            Description = dto.Description;
            ImageUrl = dto.ImageUrl;
            DetailUrl = dto.DetailUrl;
            Money1 = dto.Money1;
            Money2 = dto.Money2;
            Money3 = dto.Money3;
            Money4 = dto.Money4;
            Money5 = dto.Money5;
            Money6 = dto.Money6;
            Money7 = dto.Money7;
            Money8 = dto.Money8;
            Money9 = dto.Money9;
            Flag1 = dto.Flag1;
            Flag2 = dto.Flag2;
            Flag3 = dto.Flag3;
            Flag4 = dto.Flag4;
            Flag5 = dto.Flag5;
            Flag6 = dto.Flag6;
            Flag7 = dto.Flag7;
            Flag8 = dto.Flag8;
            Flag9 = dto.Flag9;
            Text1 = dto.Text1;
            Text2 = dto.Text2;
            Text3 = dto.Text3;
            Text4 = dto.Text4;
            Text5 = dto.Text5;
            Text6 = dto.Text6;
            Text7 = dto.Text7;
            Text8 = dto.Text8;
            Text9 = dto.Text9;
            Number1 = dto.Number1;
            Number2 = dto.Number2;
            Number3 = dto.Number3;
            Number4 = dto.Number4;
            Number5 = dto.Number5;
            Number6 = dto.Number6;
            Number7 = dto.Number7;
            Number8 = dto.Number8;
            Number9 = dto.Number9;
            Promotion = dto.Promotion;
            Inventory = dto.Inventory;
            LastUpdate = dto.LastUpdate;
            COGS = dto.COGS;
            ExcludedFromRecommendations = dto.ExcludedFromRecommendations;
        }

        public ItemInfo()
        {
            ImageUrl = string.Empty;
        }

        public Guid ItemID { get; set; }
        public string AccountID { get; set; }
        public string AccountItemID { get; set; }
        public Guid ObjectID { get; set; }
        public string CatalogID { get; set; }
        public bool Disabled { get; set; }
        public DateTime? DateIntroduced { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string DetailUrl { get; set; }
        public decimal? Money1 { get; set; }
        public decimal? Money2 { get; set; }
        public decimal? Money3 { get; set; }
        public decimal? Money4 { get; set; }
        public decimal? Money5 { get; set; }
        public decimal? Money6 { get; set; }
        public decimal? Money7 { get; set; }
        public decimal? Money8 { get; set; }
        public decimal? Money9 { get; set; }
        public bool? Flag1 { get; set; }
        public bool? Flag2 { get; set; }
        public bool? Flag3 { get; set; }
        public bool? Flag4 { get; set; }
        public bool? Flag5 { get; set; }
        public bool? Flag6 { get; set; }
        public bool? Flag7 { get; set; }
        public bool? Flag8 { get; set; }
        public bool? Flag9 { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public string Text5 { get; set; }
        public string Text6 { get; set; }
        public string Text7 { get; set; }
        public string Text8 { get; set; }
        public string Text9 { get; set; }
        public double? Number1 { get; set; }
        public double? Number2 { get; set; }
        public double? Number3 { get; set; }
        public double? Number4 { get; set; }
        public double? Number5 { get; set; }
        public double? Number6 { get; set; }
        public double? Number7 { get; set; }
        public double? Number8 { get; set; }
        public double? Number9 { get; set; }
        public double? Promotion { get; set; }
        public double? Inventory { get; set; }
        public DateTime LastUpdate { get; set; }
        public double? COGS { get; set; }
        public bool ExcludedFromRecommendations { get; set; }
    }
}