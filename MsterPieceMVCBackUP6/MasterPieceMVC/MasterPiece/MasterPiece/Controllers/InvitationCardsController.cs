using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterPiece.Models;

namespace MasterPiece.Controllers
{
    public class InvitationCardsController : Controller
    {
        private MasterPieceEntities5 db = new MasterPieceEntities5();

        // GET: InvitationCards
        public ActionResult Index()
        {
            return View(db.InvitationCards.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
