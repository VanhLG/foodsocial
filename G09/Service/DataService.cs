using G09.Models;
namespace G09.Service
{
    public class DataService : IDataService
    {
        private readonly DbG09foodContext _context;

        public DataService(DbG09foodContext context) 
        { 
            _context = context;
        }

        public List<LoaiMonAn> loaiMonAns()
        {
            var ListLoai = _context.LoaiMonAns.ToList();
            return ListLoai;
        }

    }
}
