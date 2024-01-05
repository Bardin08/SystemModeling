namespace SystemModeling.Lab3.Extensions;

public static class QueueExtensions
{
    public static async Task TransferLastItem<T>(
        this Channel<T> src,
        ChannelWriter<T> dest,
        CancellationToken ct)
    {
        T lastItem = default!;

        var amount = src.Reader.Count;
        await foreach (var item in src.Reader.ReadAllAsync(ct))
        {
            amount--;
            if (amount is 0)
            {
                lastItem = item;
                break;
            }

            await src.Writer.WriteAsync(item, ct);
        }

        await dest.WriteAsync(lastItem, ct);
    }
}