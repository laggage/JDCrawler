using System.Threading.Tasks;

namespace JDCrawler.Infrastructure.Database
{
    public class UnitOfWork
    {
        public void SaveChanges()
        {
            MobilesContext.Instance.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await MobilesContext.Instance.SaveChangesAsync();
        }
    }
}
