using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoManagement
{
    class InventoryClass
    {
        private int Id;
        private string ItemNo;
        private string Description;
        private decimal SellingPrice;
        private decimal AquiredPrice;
        private int QtyOnHand;
        private string Image;
        private int Active;
        private int UserId;
        private DateTime CreatedDateTime;
        private string Category;
        private string LeadTime;
        private string ReorderLevel;
        private string UnitsOnReoder;

        public void setId(int Id)
        {
            this.Id = Id;
        }
        public int getId()
        {
            return this.Id;
        }
        public void setItemNo(string ItemNo)
        {
            this.ItemNo = ItemNo;
        }
        public string getItemNo()
        {
            return this.ItemNo;
        }
        public void setDescription(string Description)
        {
            this.Description = Description;
        }
        public string getDescription()
        {
            return this.Description;
        }
        public void setSellingPrice(decimal SellingPrice)
        {
            this.SellingPrice = SellingPrice;
        }
        public decimal getSellingPrice()
        {
            return this.SellingPrice;
        }
        public void setAquiredPrice(decimal AquiredPrice)
        {
            this.AquiredPrice = AquiredPrice;
        }
        public decimal getAquiredPrice()
        {
            return this.AquiredPrice;
        }
        public void setQtyOnHand(int QtyOnHand)
        {
            this.QtyOnHand = QtyOnHand;
        }
        public int getQtyOnHand()
        {
            return this.QtyOnHand;
        }
        public void setImage(string Image)
        {
            this.Image = Image;
        }
        public string getImage()
        {
            return this.Image;
        }
        public void setActive(int Active)
        {
            this.Active = Active;
        }
        public int getActive()
        {
            return this.Active;
        }
        public void setUserId(int UserId)
        {
            this.UserId = UserId;
        }
        public int getUserId()
        {
            return this.UserId;
        }
        public void setCreatedDateTime(DateTime CreatedDateTime)
        {
            this.CreatedDateTime = CreatedDateTime;
        }
        public DateTime getCreatedDateTime()
        {
            return this.CreatedDateTime;
        }
        public void setUnitsOnReoder(string UnitsOnReoder)
        {
            this.UnitsOnReoder = UnitsOnReoder;
        }
        public string getUnitsOnReoder()
        {
            return this.UnitsOnReoder;
        }
        public void setReorderLevel(string ReorderLevel)
        {
            this.ReorderLevel = ReorderLevel;
        }
        public string getReorderLevel()
        {
            return this.ReorderLevel;
        }
        public void setLeadTime(string LeadTime)
        {
            this.LeadTime = LeadTime;
        }
        public string getLeadTime()
        {
            return this.LeadTime;
        }
        public void setCategory(string Category)
        {
            this.Category = Category;
        }
        public string getCategory()
        {
            return this.Category;
        }


    }
}
