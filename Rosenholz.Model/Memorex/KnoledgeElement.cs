using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model.Memorex
{
    public class KnowledgeElement
    {
        public string Link { get; set; }
        public string Searchwords { get; set; }

        public string Category { get; set; }
        public string Guid { get; set; }

        public int MatchingScore { get; private set; }

        public KnowledgeElement()
        {

        }

        public KnowledgeElement(string link, string seachwords, string category, string guid = "")
        {
            if (string.IsNullOrEmpty(guid))
                Guid = System.Guid.NewGuid().ToString();
            else
                Guid = guid;

            Link = link;
            Searchwords = seachwords;

            Category = category;
        }

        public void CalculateMatchingScore(string search)
        {
            MatchingScore = LevenshteinDistance.Compute(search, this.Searchwords);
        }
    }
}
