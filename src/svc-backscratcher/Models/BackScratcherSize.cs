using NpgsqlTypes;

namespace svc_backscratcher.Models
{
    public enum BackScratcherSize
    {
        [PgName("S")]
        S,
        [PgName("M")]
        M, 
        [PgName("L")]
        L,
        [PgName("XL")]
        XL   
    }
}