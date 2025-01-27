using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartTemplateNew.DAL.Entities.Base
{
    public class KeyedEntity<TKey> : IKeyedEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
    }
}
