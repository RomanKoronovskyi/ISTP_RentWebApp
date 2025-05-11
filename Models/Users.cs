using System.ComponentModel.DataAnnotations;

namespace RentAPIWebApp.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Повне ім'я")]
        public string UsrName { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [EmailAddress(ErrorMessage = "Невірний формат електронної пошти.")]
        [Display(Name = "Електронна пошта")]
        public string UsrEmail { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Phone(ErrorMessage = "Невірний формат номера телефону.")]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Телефон повинен починатися з +380 і містити 9 цифр після нього.")]
        [Display(Name = "Номер телефону")]
        public string UsrPhone { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [RegularExpression(@"^\d{4,}$", ErrorMessage = "Пароль має складатися лише з цифр і містити щонайменше 4 символи.")]
        [Display(Name = "Пароль")]
        public int UsrPassword { get; set; }

        public virtual ICollection<UsersHasRealtors> UserHasRealtors { get; set; } = new List<UsersHasRealtors>();
        public virtual ICollection<Favourites> Favourites { get; set; } = new List<Favourites>();
    }

}