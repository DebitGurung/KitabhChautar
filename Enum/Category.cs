using System.ComponentModel.DataAnnotations;

namespace KitabhChauta.Enum
{
    public enum Category
    {
        [Display(Name = "Best Seller")]
        BestSeller,

        [Display(Name = "Latest Books")]
        Latest,

        [Display(Name = "On Sale")]
        OnSale
    }

}
