using System.ComponentModel.DataAnnotations.Schema;

namespace CoviIDApiCore.Models.Database
{
    public abstract class BaseModel<TKey>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }
    }
}