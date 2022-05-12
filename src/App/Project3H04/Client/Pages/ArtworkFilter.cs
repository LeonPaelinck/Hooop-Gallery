using System;
using System.Collections.Generic;

namespace Project3H04.Client.Pages
{
    public class ArtworkFilter
    {
        public event Action OnArtworkFilterChanged;
        private void NotifyStateChanged() => OnArtworkFilterChanged.Invoke();

        private string artistSearchterm;
        private string artworkSearchterm;
        private List<String> mediums;
        private List<String> sizes;
        private List<String> buyingOptions;
        private int minimumPrice;
        private int maximumPrice;
        private int page;

        public string ArtistSearchterm
        {
            get => artistSearchterm;
            set
            {
                artistSearchterm = value;
                NotifyStateChanged();
            }
        }
        public string ArtworkSearchterm
        {
            get => artworkSearchterm;
            set
            {
                artworkSearchterm = value;
                NotifyStateChanged();
            }
        }
        public List<string> Mediums
        {
            get => mediums;
            set
            {
                mediums = value;
                NotifyStateChanged();
            }
        }
        public List<string> Sizes
        {
            get => sizes;
            set
            {
                sizes = value;
                NotifyStateChanged();
            }
        }
        public List<string> BuyingOptions
        {
            get => buyingOptions;
            set
            {
                buyingOptions = value;
                NotifyStateChanged();
            }
        }


        public int MinimumPrice
        {
            get => minimumPrice;
            set
            {
                minimumPrice = value;
                NotifyStateChanged();
            }
        }
        public int MaximumPrice
        {
            get => maximumPrice;
            set
            {
                maximumPrice = value;
                NotifyStateChanged();
            }
        }

        public int Page
        {
            get => page;
            set
            {
                page = value;
                NotifyStateChanged();
            }
        }
    }
}