//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace qwert1.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public int PostID { get; set; }
        public int AuthenticationID { get; set; }
    
        public virtual Authentication Authentication { get; set; }
        public virtual Post Post { get; set; }
    }
}
