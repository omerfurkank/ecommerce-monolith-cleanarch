using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Product.ValueObjects;
public class ProductImage : ValueObject
{
    public string Url { get; }

    private ProductImage(string url)
    {
        Url = url;
    }

    public static ProductImage Of(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Image URL cannot be empty.", nameof(url));

        return new ProductImage(url);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
    }
}
