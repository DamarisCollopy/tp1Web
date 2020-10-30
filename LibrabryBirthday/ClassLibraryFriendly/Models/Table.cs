using System;
using System.Collections.Generic;

namespace ClassLibraryFriendly
{
    public partial class Table
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
