using NAudio.Wave;

namespace MePoC.Utilities;
public static class StringUtils
{
    public static void RecordAudio(this string value)
    {
        if (value == "k")
        {
            string today = DateTime.UtcNow.ToString("yyyyMMdd");
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string subfolderName = "Records";
            string sessionFolderName = $"session{today}";

            // Build the full path to the subfolder
            string fullPath = Path.Combine(baseDirectory, subfolderName, sessionFolderName);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            string fileName = $"audio-{Guid.NewGuid()}.wav";
            string outputPath = Path.Combine(fullPath, fileName);

            var waveFormat = new WaveFormat(44100, 16, 2);
            using (var waveIn = new WaveInEvent())
            {
                waveIn.WaveFormat = waveFormat;
                using (var writer = new WaveFileWriter(outputPath, waveIn.WaveFormat))
                {
                    // Set up the event handler to write to the file when data is available
                    waveIn.DataAvailable += (sender, e) =>
                    {
                        writer.Write(e.Buffer, 0, e.BytesRecorded);
                    };

                    // Start recording
                    waveIn.StartRecording();
                    Console.WriteLine("Recording... Press any key to stop.");
                    Console.ReadKey();
                    waveIn.StopRecording();
                }
            }
        }
        else
        {
            Console.WriteLine("Continue with Text2Text");
        }

    }

}
