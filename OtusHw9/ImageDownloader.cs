using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtusHw9
{
    public class ImageDownloader
    {
        public event Action ImageStarted;
        public event Action ImageCompleted;
        public string _fileName { get; set; }
        public string FileName => _fileName;
        public async Task<bool> DownloadAsync(string remoteUri, string fileName, CancellationTokenSource cts )
        {
            CancellationToken token = cts.Token;
            try
            {
                ImageStarted?.Invoke();

                using (var myWebClient = new WebClient())
                {
                    token.Register(() => myWebClient.CancelAsync());
                    if (token.IsCancellationRequested)
                    {
                        await myWebClient.DownloadFileTaskAsync(remoteUri, fileName).ConfigureAwait(false);
                    }
                }

                ImageCompleted?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
