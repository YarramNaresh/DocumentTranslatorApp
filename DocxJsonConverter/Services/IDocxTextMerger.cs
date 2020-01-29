using DocxJsonConverter.Models;
using System;
using System.Collections.Generic;

namespace DocxJsonConverter.Services
{
    public interface IDocxTextMerger
    {
        IEnumerable<string> Merge(JsonConversionData jsonFileData, DocxConversionData docxFileData, Boolean ignoreHidden = false);
    }
}
