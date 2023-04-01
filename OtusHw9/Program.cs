using OtusHw9;

internal class Program
{

    static async Task Main(string[] args)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        string remoteUri = "https://img3.fonwall.ru/o/bh/russian-siberia-khakassia-qqbs.jpg?route=mid&h=750";
        ImageDownloader downloader = new ImageDownloader();
        downloader.ImageCompleted += () => Console.WriteLine("Скачивание файла закончилось");
        downloader.ImageStarted += () => Console.WriteLine("Скачивание файла началось");
        string fileName;
        var taskList = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            fileName = $"bigimage{i}.jpg";
            var t = Task.Run(async () => { return await downloader.DownloadAsync(remoteUri, fileName, cts); });
            taskList.Add(t);
        }
        Console.WriteLine("Нажмите клавишу A для выхода или любую другую клавишу для проверки статуса скачивания");
        while (!taskList.All(x => x.IsCompleted))
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.A)
            {
                Console.WriteLine("Потоки приостановлены.");
                cts.Cancel();
                Console.WriteLine("Нажмите клавишу A для выхода или любую другую клавишу для проверки статуса скачивания");
            }
        }
        Task.WaitAll(taskList.ToArray());
        Console.WriteLine("Нажмите любую клавишу для выхода");
        Console.ReadKey();
    }

}