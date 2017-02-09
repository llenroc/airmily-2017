using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace airmily.Services.ModelsExample
{
    public class User
    {
        public string Name
        {
            get;
            set;
        }

        public string Avatar
        {
            get;
            set;
        }

        public User(string name, string avatar)
        {
            Name = name;
            Avatar = avatar;
        }
    }

    public class Post
    {
        public string Title
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public string Section
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public string Avatar
        {
            get;
            set;
        }

        public string BackgroundImage
        {
            get;
            set;
        }

        public string Quote
        {
            get;
            set;
        }

        public string QuoteAuthor
        {
            get;
            set;
        }

        public string When
        {
            get;
            set;
        }

        public string Followers
        {
            get;
            set;
        }

        public string Likes
        {
            get;
            set;
        }


        public Post()
        {
        }
    }

    public class Message
    {
        public User From
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public bool HasAttachment
        {
            get;
            set;
        }

        public uint ThreadCount
        {
            get;
            set;
        }

        public string When
        {
            get;
            set;
        }

        public Boolean IsStared
        {
            get;
            set;
        }

        public Boolean IsRead
        {
            get;
            set;
        }

        public Message(
            User from,
            uint threadCount,
            bool hasAttachment,
            string when,
            string title,
            string body,
            Boolean isStared,
            Boolean isRead)
        {
            From = from;
            ThreadCount = threadCount;
            HasAttachment = hasAttachment;
            When = when;
            Title = title;
            Body = body;
            IsStared = isStared;
            IsRead = isRead;
        }
    }

    public class ChatMessage
    {
        public User From
        {
            get;
            set;
        }

        public string When
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public ChatMessage(
            User from,
            string when,
            string body
            )
        {
            From = from;
            When = when;
            Body = body;
        }
    }

    public class Product
    {
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Price
        {
            get;
            set;
        }

        public string Image
        {
            get;
            set;
        }

        public string Manufacturer
        {
            get { return "UXDIVERS"; }
            set { }
        }

        public string ThumbnailHeight
        {
            get;
            set;
        }

        public Product()
        {
        }
    }

    public static class SampleData
    {
        public static string[] Names = {
            "Pat Davies",
            "Janis Spector",
            "Regina Joplin",
            "Jaco Morrison",
            "Margaret Whites",
            "Skyler Harrisson",
            "Al Pastorius",
        };

        public static List<string> SocialImageGalleryItems = new List<string>() {
            "social_album_1.jpg",
            "social_album_2.jpg",
            "social_album_3.jpg",
            "social_album_4.jpg",
            "social_album_5.jpg",
            "social_album_6.jpg",
            "social_album_7.jpg",
            "social_album_8.jpg",
            "social_album_9.jpg"
        };

        public static List<string> ArticlesImagesList = new List<string>() {
            "article_image_0.jpg",
            "article_image_1.jpg",
            "article_image_2.jpg",
            "article_image_3.jpg",
            "article_image_4.jpg",
            "article_image_5.jpg"
        };

        public static List<string> UsersImagesList = new List<string>() {
            "friend_thumbnail_27.jpg",
            "friend_thumbnail_31.jpg",
            "friend_thumbnail_34.jpg",
            "friend_thumbnail_55.jpg",
            "friend_thumbnail_71.jpg",
            "friend_thumbnail_75.jpg",
            "friend_thumbnail_93.jpg",
        };

        public static List<string> DashboardImagesList = new List<string>() {
            "dashboard_thumbnail_0.jpg",
            "dashboard_thumbnail_1.jpg",
            "dashboard_thumbnail_2.jpg",
            "dashboard_thumbnail_3.jpg",
            "dashboard_thumbnail_4.jpg",
            "dashboard_thumbnail_5.jpg",
            "dashboard_thumbnail_6.jpg",
            "dashboard_thumbnail_7.jpg",
            "dashboard_thumbnail_8.jpg",
        };

        public static List<string> ProductsImagesList = new List<string>() {
            "product_item_0.jpg",
            "product_item_1.jpg",
            "product_item_2.jpg",
            "product_item_3.jpg",
            "product_item_4.jpg",
            "product_item_5.jpg",
            "product_item_6.jpg",
            "product_item_7.jpg",
        };

        public static List<User> Users = new List<User> {
            new User( Names[0], UsersImagesList[0] ),
            new User( Names[1], UsersImagesList[1] ),
            new User( Names[2], UsersImagesList[2] ),
            new User( Names[3], UsersImagesList[3] ),
            new User( Names[4], UsersImagesList[4] ),
            new User( Names[5], UsersImagesList[5] ),
            new User( Names[6], UsersImagesList[6] ),
        };

        public static List<User> Friends = Users;


        public static List<Post> Posts = new List<Post> {

            new Post {
                Title           = "The face of Wild Nature",
                Body            = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent et aliquet nunc. \nSed ultricies sed augue sit amet maximus. In vel tellus sed ipsum volutpat venenatis et sit amet diam. Suspendisse feugiat mollis nibh, in facilisis diam convallis sit amet. \n\nMaecenas lectus turpis, rhoncus et est at, lacinia placerat urna. Praesent malesuada consectetur justo, scelerisque fermentum enim lobortis ullamcorper. Duis commodo sit amet ligula vitae luctus. Nulla commodo ipsum a lorem efficitur luctus.",
                Section         = "NATURE",
                Author          = "UXDIVERS",
                Avatar          = SampleData.Friends[3].Avatar,
                BackgroundImage = SampleData.ArticlesImagesList[0],
                Quote           = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                QuoteAuthor     = SampleData.Friends[3].Name,
                When            = "JUN 15, 2015",
                Likes           = "92",
                Followers       = "2K",
            },
            new Post {
                Title           = "Upercut to glory",
                Body            = "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors. This is how we design a great article sample.",
                Section         = "SPORTS",
                Author          = SampleData.Friends[0].Name,
                Avatar          = SampleData.Friends[0].Avatar,
                BackgroundImage = SampleData.ArticlesImagesList[1],
                Quote           = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                QuoteAuthor     = SampleData.Friends[0].Name,
                When            = "JUN 15, 2015",
                Likes           = "92",
                Followers       = "2K",

            },
            new Post {
                Title           = "Street and its influence on design",
                Body            = "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors. This is how we design a great article sample.",
                Section         = "DESIGN",
                Author          = SampleData.Friends[2].Name,
                Avatar          = SampleData.Friends[2].Avatar,
                BackgroundImage = SampleData.ArticlesImagesList[2],
                Quote           = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                QuoteAuthor     = SampleData.Friends[2].Name,
                When            = "JUN 15, 2015",
                Likes           = "92",
                Followers       = "2K",

            },
            new Post {
                Title           = "The time for new adventures",
                Body            = "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors. This is how we design a great article sample.",
                Section         = "TRAVEL",
                Author          = "UXDIVERS",
                Avatar          = SampleData.Friends[1].Avatar,
                BackgroundImage = SampleData.ArticlesImagesList[3],
                Quote           = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                QuoteAuthor     = SampleData.Friends[1].Name,
                When            = "JUN 15, 2015",
                Likes           = "92",
                Followers       = "2K",

            },
            new Post {
                Title           = "Shooting like pros, learn how",
                Body            = "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors. This is how we design a great article sample.",
                Section         = "FREE TIME",
                Author          = SampleData.Friends[4].Name,
                Avatar          = SampleData.Friends[4].Avatar,
                BackgroundImage = SampleData.ArticlesImagesList[4],
                Quote           = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                QuoteAuthor     = SampleData.Friends[0].Name,
                When            = "JUN 15, 2015",
                Likes           = "92",
                Followers       = "2K",

            },

            new Post {
                Title           = "The search for healthy food begins",
                Body            = "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors. This is how we design a great article sample.",
                Section         = "HEALTH",
                Author          = SampleData.Friends[5].Name,
                Avatar          = SampleData.Friends[5].Avatar,
                BackgroundImage = SampleData.ArticlesImagesList[5],
                Quote           = "Donec euismod nulla et sem lobortis ultrices. Cras id imperdiet metus. Sed congue luctus arcu.",
                QuoteAuthor     = SampleData.Friends[5].Name,
                When            = "JUN 15, 2015",
                Likes           = "92",
                Followers       = "2K",

            },
        };

        public static List<Message> Messages = new List<Message> {
            new Message(
                Friends[5],
                7,
                true,
                "July 7",
                "Hey check this out!",
                "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors.",
                true,
                true
            ),

            new Message(
                Friends[1],
                3,
                false,
                "Yesterday",
                "Artina is awesome...you'll love it",
                "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors.",
                false,
                true
            ),

            new Message(
                Friends[2],
                1,
                true,
                "July 7",
                "Artina is awesome...you'll love it",
                "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors.",
                false,
                false
            ),

            new Message(
                Friends[3],
                2,
                true,
                "July 7",
                "Artina is awesome...you'll love it",
                "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors.",
                false,
                false
            ),

            new Message(
                Friends[4],
                10,
                false,
                "3 minutes ago",
                "Artina is awesome...you'll love it",
                "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors.",
                true,
                false
            ),

            new Message(
                Friends[0],
                5,
                false,
                "July 7",
                "Artina is awesome...you'll love it",
                "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors.",
                true,
                true
            ),

            new Message(
                Friends[6],
                7,
                false,
                "July 7",
                "Artina is awesome...you'll love it",
                "In connection with this appellative of 'Whalebone whales,' it is of great leap of yer happiness leadership colors.",
                true,
                false
            ),

        };

        public static List<Message> Comments = Messages.GetRange((Messages.Count() / 2), (Messages.Count() / 2));

        public static List<User> SmallUserList = Users.GetRange(0, 2);

        public static List<Post> SmallPostList = Posts.GetRange(0, 1);

        public static List<ChatMessage> ChatMessagesList = new List<ChatMessage> {
            new ChatMessage (
                Friends [5],
                "July 7",
                "Hey buddy :), what's up? I'm currently working on this amazing stuff called Grial. Have you heard about it? You shoud give it a try....it really rocks!!!!!."
            ),
            new ChatMessage (
                Friends [5],
                "July 7",
                "You should give it a try!"
            ),
            new ChatMessage (
                Friends [1],
                "July 7",
                "Wooow! Didn't know this exist!! Really cool stuff!"
            ),

            new ChatMessage (
                Friends [1],
                "July 7",
                "I was wondering if something like this existed. This will save hundred of hours. I rather be skateboarding with my friends instead of compiling every little visual change." +
                "Thanks for sharing!"
            ),

            new ChatMessage (
                Friends [5],
                "July 7",
                "No problem! I hope you can find it useful. It really makes the difference to me."
            ),

            new ChatMessage (
                Friends [1],
                "July 7",
                "Ok thanks and thanks again!! This is really awesome"
            ),

            new ChatMessage (
                Friends [5],
                "July 7",
                "Indeed."
            ),

            new ChatMessage (
                Friends [1],
                "July 7",
                "C u later tonight at Gillian Japi party, right?"
            ),

            new ChatMessage (
                Friends [5],
                "July 7",
                "For sure! See you later :)"
            ),
        };


        public static List<Product> Products = new List<Product> {
            new Product {
                Name            = "Logo Tee",
                Description     = "Cotton/ploy blend lends for ultimate comfort.",
                Image           = SampleData.ProductsImagesList[0],
                Price           = "$39",
                ThumbnailHeight  = "100"
            },

            new Product {
                Name            = "Big Logo Shirt",
                Description     = "This Logo UA Tech T-Shirt is built with a system that wicks away sweat to keep your little one dry and comfortable.",
                Image           = SampleData.ProductsImagesList[1],
                Price           = "$29",
                ThumbnailHeight  = "100"
            },

            new Product {
                Name            = "Classic Tee",
                Description     = "The V-Neck Embroidered T-Shirt keeps you looking fresh with its simple yet classic look. 100% cotton. Imported.",
                Image           = SampleData.ProductsImagesList[2],
                Price           = "$39",
                ThumbnailHeight  = "100"
            },

            new Product {
                Name            = "Loose Fit Tee",
                Description     = "Our newest swim tees with a much looser fit than traditional rash guard for yet more comfort and versatility, is well known for great fit, function and colors.",
                Image           = SampleData.ProductsImagesList[3],
                Price           = "$29",
                ThumbnailHeight  = "100"
            },

            new Product {
                Name            = "Cotton Tee",
                Description     = "Standard fit tee shirt, graphic printed with soft hand ink",
                Image           = SampleData.ProductsImagesList[4],
                Price           = "$29",
                ThumbnailHeight = "100"
            },

            new Product {
                Name            = "Sports Tee",
                Description     = "Comfortable fit whilst the flat-seam construction helps to minimise chafing and they also feature side panels, enhancing your range of movement.",
                Image           = SampleData.ProductsImagesList[5],
                Price           = "$39",
                ThumbnailHeight  = "100"
            },

            new Product {
                Name            = "Classic T-Shirt",
                Description     = "All you need for a comfort day.",
                Image           = SampleData.ProductsImagesList[6],
                Price           = "$29",
                ThumbnailHeight = "100"
            },

            new Product {
                Name            = "Product name 8",
                Description     = "The long sleeves provide extra coverage and warmth and the sweat-wicking Dri-FIT fabric keeps you comfortable. The lightweight layer contours to your body and strategic mesh panels increase airflow. 92% polyester, 8% spandex.",
                Image           = SampleData.ProductsImagesList[7],
                Price           = "$29",
                ThumbnailHeight = "100"
            }
        };
    }

    public class SampleCoordinator
    {
        public static event EventHandler<SampleEventArgs> SelectedSampleChanged;
        public static event EventHandler<EventArgs> PresentMainMenuOnAppearance;
        public static event EventHandler<SampleEventArgs> SampleSelected;

        private static Sample _selectedSample = null;

        public static void RaisePresentMainMenuOnAppearance()
        {
            if (PresentMainMenuOnAppearance != null)
            {
                PresentMainMenuOnAppearance(typeof(SampleCoordinator), null);
            }
        }

        public static void RaiseSampleSelected(Sample sample)
        {
            if (SampleSelected != null)
            {
                SampleSelected(typeof(SampleCoordinator), new SampleEventArgs(sample));
            }
        }

        public static Sample SelectedSample
        {
            get
            {
                return _selectedSample;
            }

            set
            {
                if (_selectedSample != value)
                {
                    _selectedSample = value;

                    if (SelectedSampleChanged != null)
                    {
                        SelectedSampleChanged(typeof(SampleCoordinator), new SampleEventArgs(value));
                    }
                }
            }
        }
    }

    public class SampleEventArgs : EventArgs
    {
        private readonly Sample _sample;

        public SampleEventArgs(Sample newSample)
        {
            _sample = newSample;
        }

        public Sample Sample
        {
            get
            {
                return _sample;
            }
        }
    }

    public class Sample
    {
        private readonly string _name;
        private readonly bool _modal;
        private readonly char _icon;
        private readonly Type _pageType;
        private readonly string _backgroundImage;
        private readonly bool _justNotifyNavigateIntent;
        private readonly Action<INavigation> _customNavigation;

        public Sample(string name, Type pageType, string backgroundImage, char icon = '\uf054', bool modal = false, bool justNotifyNavigateIntent = false, Action<INavigation> customNavigation = null)
        {
            _name = name;
            _pageType = pageType;
            _icon = icon;
            _backgroundImage = backgroundImage;
            _modal = modal;
            _justNotifyNavigateIntent = justNotifyNavigateIntent;
            _customNavigation = customNavigation;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public char Icon
        {
            get
            {
                return _icon;
            }
        }

        public string BackgroundImage
        {
            get
            {
                return _backgroundImage;
            }
        }

        public async Task NavigateToSample(INavigation navigation)
        {
            SampleCoordinator.RaiseSampleSelected(this);

            if (_justNotifyNavigateIntent)
            {
                return;
            }

            if (_customNavigation != null)
            {
                _customNavigation(navigation);
                return;
            }

            int popCount = 0;
            int firstPageToPopIndex = 0;

            for (int i = navigation.NavigationStack.Count - 1; i >= 0; i--)
            {
                if (navigation.NavigationStack[i].GetType() == _pageType)
                {
                    firstPageToPopIndex = i + 1;
                    popCount = navigation.NavigationStack.Count - 1 - i;
                    break;
                }
            }

            if (popCount > 0)
            {
                for (int i = 1; i < popCount; i++)
                {
                    navigation.RemovePage(navigation.NavigationStack[firstPageToPopIndex]);
                }

                await navigation.PopAsync();

                return;
            }

            var page = CreateContentPage();

            if (_modal)
            {
                await navigation.PushModalAsync(new NavigationPage(page));
            }
            else
            {
                await navigation.PushAsync(page);
            }
        }

        private Page CreateContentPage()
        {
            var page = Activator.CreateInstance(_pageType) as Page;

            System.Diagnostics.Debug.Assert(page != null);

            return page;
        }

        public Type PageType
        {
            get
            {
                return _pageType;
            }
        }
    }

    public class SampleCategory
    {
        public string Name { get; set; }

        public Color BackgroundColor { get; set; }

        public String BackgroundImage { get; set; }

        public List<Sample> SamplesList { get; set; }

        public char Icon { get; set; }
    }

    public static class SamplesDefinition
    {
        private static ObservableCollection<SampleCategory> _samplesCategoryList;
        private static Dictionary<string, SampleCategory> _samplesCategories;
        private static List<Sample> _allSamples;
        private static List<SampleGroup> _samplesGroupedByCategory;

        public static string[] _categoriesColors = {
            "#c01e5c",
            "#ab1958",
            "#861350",
            "#473957",
            "#554666",
            "#5a5586",
            "#4d75a5",
            "#509acb",
            "#5abaeb"
        };

        public static ObservableCollection<SampleCategory> SamplesCategoryList
        {
            get
            {
                if (_samplesCategoryList == null)
                    InitializeSamples();

                return _samplesCategoryList;
            }
        }

        public static Dictionary<string, SampleCategory> SamplesCategories
        {
            get
            {
                if (_samplesCategories == null)
                {
                    InitializeSamples();
                }

                return _samplesCategories;
            }
        }

        public static List<Sample> AllSamples
        {
            get
            {
                if (_allSamples == null)
                {
                    InitializeSamples();
                }
                return _allSamples;
            }
        }

        public static List<SampleGroup> SamplesGroupedByCategory
        {
            get
            {
                if (_samplesGroupedByCategory == null)
                {
                    InitializeSamples();
                }

                return _samplesGroupedByCategory;
            }
        }


        internal static void InitializeSamples()
        {
            var categories = new Dictionary<string, SampleCategory>();

            categories.Add(
                "SOCIAL",
                new SampleCategory
                {
                    Name = "Social",
                    BackgroundColor = Color.FromHex(_categoriesColors[0]),
                    BackgroundImage = SampleData.DashboardImagesList[6],
                    Icon = '\uf0e6',
                    SamplesList = new List<Sample> {
                        new Sample("User Profile", null, SampleData.DashboardImagesList[6], '\uf007'),
                        new Sample("Social", null, SampleData.DashboardImagesList[6], '\uf0e6'),
                        new Sample("Social Variant", null, SampleData.DashboardImagesList[6], '\uf0e6'),
                    }
                }
            );

            categories.Add(
                "ARTICLES",
                new SampleCategory
                {
                    Name = "Articles",
                    BackgroundColor = Color.FromHex(_categoriesColors[1]),
                    BackgroundImage = SampleData.DashboardImagesList[4],
                    Icon = '\uf0f6',
                    SamplesList = new List<Sample> {
                        new Sample("Article View", null, SampleData.DashboardImagesList[4], '\uf0f6'),
                        new Sample("Articles List", null, SampleData.DashboardImagesList[4], '\uf0f6'),
                        new Sample("Articles List Variant", null, SampleData.DashboardImagesList[4], '\uf0f6'),
                        new Sample("Articles Feed", null, SampleData.DashboardImagesList[4], '\uf0f6'),
                    }
                }
            );

            categories.Add(
                "DASHBOARD",
                new SampleCategory
                {
                    Name = "Dashboards",
                    BackgroundColor = Color.FromHex(_categoriesColors[2]),
                    BackgroundImage = SampleData.DashboardImagesList[3],
                    Icon = '\uf009',
                    SamplesList = new List<Sample> {
                        new Sample("Icons Dashboard", null, SampleData.DashboardImagesList[3], '\uf009'),
                        new Sample("Flat Dashboard", null, SampleData.DashboardImagesList[3], '\uf009'),
                        new Sample("Images Dashboard", null, SampleData.DashboardImagesList[3], '\uf009'),
                    }
                }
            );


            categories.Add(
                "NAVIGATION",
                new SampleCategory
                {
                    Name = "Navigation",
                    BackgroundColor = Color.FromHex(_categoriesColors[3]),
                    BackgroundImage = SampleData.DashboardImagesList[2],
                    Icon = '\uf0c9',
                    SamplesList = new List<Sample> {
                        new Sample("RootPage", null, SampleData.DashboardImagesList[2], '\uf0c9', false, true),
                        new Sample("Categories List Flat", null, SampleData.DashboardImagesList[2], '\uf03a'),
                        new Sample("Image Categories", null, SampleData.DashboardImagesList[2], '\uf03a'),
                        new Sample("Icon Categories", null, SampleData.DashboardImagesList[2], '\uf03a'),
                        new Sample("Custom NavBar", null, SampleData.DashboardImagesList[2], '\uf022'),
                    }
                }
            );

            categories.Add(
                "LOGINS",
                new SampleCategory
                {
                    Name = "Logins",
                    BackgroundColor = Color.FromHex(_categoriesColors[4]),
                    BackgroundImage = SampleData.DashboardImagesList[5],
                    Icon = '\uf023',
                    SamplesList = new List<Sample> {
                        new Sample("Login", null, SampleData.DashboardImagesList[5], '\uf023', true),
                        new Sample("Sign Up", null, SampleData.DashboardImagesList[5], '\uf046', true),
                        new Sample("Password Recovery", null, SampleData.DashboardImagesList[5], '\uf0e2', true),
                    }
                }
            );

            categories.Add(
                "ECOMMERCE",
                new SampleCategory
                {
                    Name = "Ecommerce",
                    BackgroundColor = Color.FromHex(_categoriesColors[5]),
                    BackgroundImage = SampleData.DashboardImagesList[1],
                    Icon = '\uf07a',
                    SamplesList = new List<Sample> {
                        new Sample("Products Grid", null, SampleData.DashboardImagesList[1] , '\uf0db'),
                        new Sample("Products Grid Variant", null, SampleData.DashboardImagesList[1] , '\uf0db'),
                        new Sample("Product Item View", null, SampleData.DashboardImagesList[1], '\uf06b'),
                        new Sample("Products Carousel", null, SampleData.DashboardImagesList[1], '\uf06b'),
                    }
                }
            );

            categories.Add(
                "WALKTROUGH",
                new SampleCategory
                {
                    Name = "Walkthroughs",
                    BackgroundColor = Color.FromHex(_categoriesColors[6]),
                    BackgroundImage = SampleData.DashboardImagesList[7],
                    Icon = '\uf0d0',
                    SamplesList = new List<Sample> {
                        new Sample("Walkthrough", null, SampleData.DashboardImagesList[7], '\uf0d0', true),
                        new Sample("Walkthrough Variant", null, SampleData.DashboardImagesList[7], '\uf0d0', true),
                    }
                }
            );

            categories.Add(
                "MESSAGES",
                new SampleCategory
                {
                    Name = "Messages",
                    BackgroundColor = Color.FromHex(_categoriesColors[7]),
                    BackgroundImage = SampleData.DashboardImagesList[8],
                    Icon = '\uf003',
                    SamplesList = new List<Sample> {
                        new Sample("Messages", null, SampleData.DashboardImagesList[8], '\uf003'),
                        new Sample("Chat Messages List", null, SampleData.DashboardImagesList[8], '\uf0e6'),
                    }
                }
            );

            categories.Add(
                "THEME",
                new SampleCategory
                {
                    Name = "Grial Theme",
                    BackgroundColor = Color.FromHex(_categoriesColors[8]),
                    BackgroundImage = SampleData.DashboardImagesList[0],
                    Icon = '\uf1fc',
                    SamplesList = new List<Sample> {
                        new Sample("Native controls", null, SampleData.DashboardImagesList[0], '\uf1fc'),
                        new Sample("Custom Renderers", null, SampleData.DashboardImagesList[0], '\uf1fc'),
                        new Sample("Grial Common Views", null, SampleData.DashboardImagesList[0], '\uf1fc'),
                        new Sample("Settings Page", null, SampleData.DashboardImagesList[0], '\uf085'),
                        new Sample("About", null, SampleData.DashboardImagesList[0], '\uf128'),
                        new Sample("Tabs", null, SampleData.DashboardImagesList[0], '\uf114'),
                    }
                }
            );


            _samplesCategories = categories;

            _samplesCategoryList = new ObservableCollection<SampleCategory>();

            foreach (var sample in _samplesCategories.Values)
            {
                _samplesCategoryList.Add(sample);
            }

            _allSamples = new List<Sample>();

            _samplesGroupedByCategory = new List<SampleGroup>();

            foreach (var sampleCategory in SamplesCategories.Values)
            {

                var sampleItem = new SampleGroup(sampleCategory.Name.ToUpper());

                foreach (var sample in sampleCategory.SamplesList)
                {
                    _allSamples.Add(sample);
                    sampleItem.Add(sample);
                }

                _samplesGroupedByCategory.Add(sampleItem);
            }
        }

        private static void RootPageCustomNavigation(INavigation navigation)
        {
            SampleCoordinator.RaisePresentMainMenuOnAppearance();
            navigation.PopToRootAsync();
        }
    }

    public class SampleGroup : List<Sample>
    {
        private readonly string _name;

        public SampleGroup(string name)
        {
            _name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
