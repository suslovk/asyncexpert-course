using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pipelines
{
    public class PipeLineCounter
    {
        public async Task<int> CountLines(Uri uri)
        {
            using var client = new HttpClient();
            await using var stream = await client.GetStreamAsync(uri);

            var pipe = new Pipe();
            var writer = FillPipeAsync(stream, pipe.Writer);
            var reader = ReadPipeAsync(pipe.Reader);
            await Task.WhenAll(writer, reader);

            return reader.Result;
        }

        private static async Task FillPipeAsync(Stream stream, PipeWriter writer)
        {
            while (true)
            {
                var memory = writer.GetMemory(512);
                var read = await stream.ReadAsync(memory);
                if (read == 0)
                    break;

                writer.Advance(read);
                var flushResult = await writer.FlushAsync();
                if (flushResult.IsCompleted)
                    break;
            }

            await writer.CompleteAsync();
        }

        private static async Task<int> ReadPipeAsync(PipeReader reader)
        {
            var linesCount = 0;
            while (true)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;
                linesCount += CountNewLines(buffer);
                if (result.IsCompleted)
                    break;
            }

            await reader.CompleteAsync();
            return linesCount;
        }

        private static int CountNewLines(in ReadOnlySequence<byte> buffer)
        {
            var counter = 0;
            var sequence = new SequenceReader<byte>(buffer);
            while (sequence.TryAdvanceTo((byte) '\n'))
                counter++;

            return counter;
        }
    }
}