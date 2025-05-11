using System.ComponentModel.DataAnnotations;

namespace RentAPIWebApp.Models
{
    public class Districts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [MinLength(5, ErrorMessage = "Назва району повинна містити щонайменше 5 символів")]
        [Display(Name = "Назва району")]
        public string DsName { get; set; }

        public virtual ICollection<Flats> Flats { get; set; } = new List<Flats>();
    }

}