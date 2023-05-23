using Castle.Components.DictionaryAdapter;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    [Table("Department")]
    public class Department
    {
        [Key("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

