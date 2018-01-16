﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CyberArena.DAL;
using CyberArena.Models;
using AutoMapper;
using CyberArena.CustomExtensions;
using CyberArena.Repository;

namespace CyberArena.Controllers
{
    public class PlayersController : Controller
    {

        private UnitOfWork unit=new UnitOfWork();

        
        // GET: Players
        public ActionResult Index(string searchString)
        {

            var players = unit.PlayerRepository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                players = unit.PlayerRepository.FindBy(s => s.Nickname.Contains(searchString));
            }

            List<PlayerView> playersViews = players.Map<List<PlayerView>>();

            return View(playersViews);
        }

        // GET: Players/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = unit.PlayerRepository.FindByID(id);
            
               
            if (player == null)
            {
                return HttpNotFound();
            }

            PlayerView playerView = player.Map<PlayerView>();
            return View(playerView);          
            
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<Team> teams = unit.TeamRepository.GetAll();
            for (int i=0;i<teams.Count;i++)
            {
                items.Add(new SelectListItem { Text = teams[i].Name, Value = teams[i].TeamID.ToString() });
            }
            ViewBag.Teams = items;
            return View();
        }

        // POST: Players/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlayerID,LastName,FirstName,Nickname,Discipline,MMR,TeamID")] PlayerView playerModel)
        {
            Player player=new Player();
            if (ModelState.IsValid)
            {
                player = playerModel.Map<Player>();
                unit.PlayerRepository.Add(player);
                unit.Save();
                return RedirectToAction("Index");
            }

            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = unit.PlayerRepository.FindByID(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            PlayerView playerModel=player.Map<PlayerView>();

            List<SelectListItem> items = new List<SelectListItem>();
            List<Team> teams = unit.TeamRepository.GetAll();
            for (int i = 0; i < teams.Count; i++)
            {
                items.Add(new SelectListItem { Text = teams[i].Name, Value = teams[i].TeamID.ToString() });
            }
            ViewBag.Teams = items;

            return View(playerModel);
        }

        // POST: Players/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlayerID,LastName,FirstName,Nickname,Discipline,MMR,TeamID")] PlayerView playerModel)
        {
            Player player = new Player();
            if (ModelState.IsValid)
            {
                player = playerModel.Map<Player>();
                unit.PlayerRepository.Edit(player);
                unit.Save();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = unit.PlayerRepository.FindByID(id);
            if (player == null)
            {
                return HttpNotFound();
            }

            PlayerView playerView = player.Map<PlayerView>();

            return View(playerView);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = unit.PlayerRepository.FindByID(id);
            unit.PlayerRepository.Delete(player);
            unit.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unit.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
