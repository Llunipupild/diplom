using Core.XmlReader.Config;

namespace Descriptors.Interface
{
    public interface IDescriptor
    {
        public void SetData(Configuration config);
        public void SetData(string englishWord, string russianWord, string iconPath);
    }
}