using System.Threading.Tasks;

namespace TinyLittleMvvm {
    public interface IOnLoadedHandler {
        Task OnLoadedAsync();
    }
}