using Core.Repository;

namespace Time
{
    public class TimeDeckRepository : PlayerPrefsRepository<TimeDeckModel>
    {
        public TimeDeckRepository() : base("timeRepository")
        {
        }
    }
}