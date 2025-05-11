using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace RentAPIWebApp.Models
{
    public class UsersHasRealtors
    {
        public int Id { get; set; }

        public int UsrId { get; set; }
        [JsonIgnore]
        [BindNever]
        public virtual Users ?Usr { get; set; }

        public int RlId { get; set; }
        [JsonIgnore]
        [BindNever]
        public virtual Realtors ?Rl { get; set; }
    }
}