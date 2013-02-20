using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Facebook;
using FacebookOptimization.Model;

namespace FacebookOptimization.ViewModels
{
    public class FriendsViewModel : INotifyPropertyChanged
    {
        private string message;
        private string AccessToken = "<COPY_ACCESS_TOKEN_HERE>";

        public string Message
        {
            get { return message; }
            set
            {
                message = value;

                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Message"));
            }
        }

        public ObservableCollection<Friend> Friends { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FriendsViewModel()
        {
            this.Friends = new ObservableCollection<Friend>();

            this.SaveETag(String.Empty);
        }

        public void LoadFriends()
        {
            try
            {
                var service = new FacebookClient(this.AccessToken);

                var etag = this.ReadETag();

                dynamic friendsData = service.Get("me/friends", new { fields = "name,picture", _etag_ = etag });

                if (!friendsData.ContainsKey("body"))
                {
                    this.Message = string.Format("No changes from last time: {0}\nTry again after to add new friends on facebook", DateTime.Now.ToLongTimeString());
                    return;
                }

                this.SaveETag(friendsData.headers.ETag);

                this.RefreshFriends(friendsData.body.data);

                this.Message = string.Format("Friends Loaded: {0}", this.Friends.Count);
            }
            catch (Exception e)
            {
                this.Message = string.Format("Error: {0}", e.Message);
            }
        }

        private void RefreshFriends(dynamic friendListData)
        {
            this.Friends.Clear();
            foreach (var friend in friendListData)
            {
                var pictureUrl = friend.ContainsKey("picture") ? friend.picture.data.url : string.Empty;

                this.Friends.Add(new Friend() { Name = friend.name, PictureUrl = pictureUrl });
            }
        }

        private void SaveETag(string ETag)
        {
            File.WriteAllText("ETag.txt", ETag);
        }

        private string ReadETag()
        {
            return File.ReadAllText("ETag.txt");
        }
    }
}
