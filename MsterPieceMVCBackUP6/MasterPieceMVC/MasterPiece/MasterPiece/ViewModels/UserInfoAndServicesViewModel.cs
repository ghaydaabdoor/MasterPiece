using MasterPiece.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterPiece.ViewModels
{
    public class UserInfoAndServicesViewModel
    {
        public User user {  get; set; }
        public IEnumerable<Booking> booking { get; set; }
        public IEnumerable<BookingCloth> bookCloth { get; set; }
        public IEnumerable<BookingPhotographer> bookPhotographer { get; set; }
        public IEnumerable<InvitationCardForm> invitationCardForm { get; set; }
    }
}