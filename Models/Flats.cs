using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace RentAPIWebApp.Models
{
    public class Flats
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [StringLength(50, ErrorMessage = "Адреса не може перевищувати 50 символів")]
        [Display(Name = "Адреса")]
        public string FlAddr { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(10, 1000, ErrorMessage = "Площа має бути в межах від 10 до 1000 кв. м")]
        [Display(Name = "Загальна площа")]
        public double FlArea { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(1, 10, ErrorMessage = "Кількість кімнат має бути від 1 до 10")]
        [Display(Name = "Кількість кімнат")]
        public int FlRooms { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Ціна має бути додатньою")]
        [Display(Name = "Ціна")]
        public decimal FlPrice { get; set; }

        public int DsId { get; set; }
        [JsonIgnore]
        [BindNever]
        public virtual Districts ?Ds { get; set; }

        public virtual ICollection<Favourites> Favourites { get; set; } = new List<Favourites>();
    }

}