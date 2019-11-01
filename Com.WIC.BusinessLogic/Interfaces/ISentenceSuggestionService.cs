using Com.WIC.BusinessLogic.Models;
using System.Collections.Generic;

namespace Com.WIC.BusinessLogic.Interfaces
{
    interface ISentenceSuggestionService
    {
        IEnumerable<SentenceModel> Suggest(string keyword);
    }
}
