using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TesteMixFiscal.Context;
using TesteMixFiscal.Models;

namespace TesteMixFiscal.Controllers
{
    public class NotaController : Controller
    {
        private MixFiscalContext db = new MixFiscalContext();

        // GET: Nota
        public ActionResult Index()
        {
            var nota = db.Nota.Include(n => n.Tipo);
            return View(nota.ToList());
        }

        //[HttpPost]
        public ActionResult Pesquisa(string busca = null, int tipo = 0, List<Nota> oNota = null)
        {
            if (busca != null && tipo == 1)
            {
                return View(db.Nota.Include(n => n.Tipo).Where(x => x.TipoId == tipo));
            }
            else if (busca != null && tipo == 2)
            {
                return View(db.Nota.Include(n => n.Tipo).Where(x => x.TipoId == tipo));
            }
            else if (busca != null && tipo == 3)
            {
                return View(db.Nota.Include(n => n.Tipo).Where(x => x.Descricao.ToUpper().Contains(busca.ToUpper())).ToList());
            }

            var nota = oNota != null ? oNota : db.Nota.Include(n => n.Tipo).ToList();
            return View(nota.ToList());
        }

        public ActionResult Organizar(string nota)
        {
            int indexNfe = 0;
            int indexSped = 0;

            var oRegistroOrganizado = new List<Nota>();
            var registro = db.Nota.Include(x => x.Tipo).Where(x => x.Descricao == nota).ToList();

            var oNfe = registro.Where(x => x.TipoId == 1).ToList();
            var oSped = registro.Where(x => x.TipoId == 2).ToList();

            foreach (var nfe in oNfe)
            {
                indexSped = 0;
                indexNfe++;
                foreach (var sped in oSped)
                {
                    indexSped++;
                    if (nfe.CodItem == sped.CodItem)
                    {
                        nfe.DescCompl = sped.DescCompl;
                        nfe.Ean = sped.Ean;
                        nfe.Qtd = sped.Qtd;
                        nfe.VlItem = sped.VlItem;
                        nfe.Vinculo = indexSped;
                    }
                    if (nfe.DescCompl == sped.DescCompl)
                    {
                        nfe.CodItem = sped.CodItem;
                        nfe.Ean = sped.Ean;
                        nfe.Qtd = sped.Qtd;
                        nfe.VlItem = sped.VlItem;
                        nfe.Vinculo = indexSped;
                    }
                    else if (nfe.Ean == sped.Ean)
                    {
                        nfe.CodItem = sped.CodItem;
                        nfe.DescCompl = sped.DescCompl;
                        nfe.Qtd = sped.Qtd;
                        nfe.VlItem = sped.VlItem;
                        nfe.Vinculo = indexSped;
                    }
                    else if (nfe.Qtd == sped.Qtd && nfe.VlItem == sped.VlItem)
                    {
                        OrganizarPorDescricao(nfe, sped, indexSped);
                    }                    
                }
            }

            oRegistroOrganizado.AddRange(oNfe);
            oRegistroOrganizado.AddRange(oSped);

            return View(oRegistroOrganizado);
        }
        public void OrganizarPorDescricao(Nota nfe, Nota sped, int indexSped)
        {
            var descricaoSplit = nfe.DescCompl.Split(' ');

            foreach (var item1 in descricaoSplit.ToList())
            {
                foreach (var item2 in descricaoSplit.ToList())
                {
                    if (sped.DescCompl.Contains(item1) && sped.DescCompl.Contains(item2))
                    {
                        nfe.CodItem = sped.CodItem;
                        nfe.DescCompl = sped.DescCompl;
                        nfe.Ean = sped.Ean;
                        nfe.Qtd = sped.Qtd;
                        nfe.VlItem = sped.VlItem;
                        nfe.Vinculo = indexSped;
                    }
                }
            }
        }

        // GET: Nota/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Nota.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            return View(nota);
        }

        // GET: Nota/Create
        public ActionResult Create()
        {
            ViewBag.TipoId = new SelectList(db.Tipo, "TipoId", "Nome");
            return View();
        }

        // POST: Nota/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NItemId,Descricao,CodItem,DescCompl,Ean,Qtd,VlItem,TipoId")] Nota nota)
        {
            if (ModelState.IsValid)
            {
                db.Nota.Add(nota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TipoId = new SelectList(db.Tipo, "TipoId", "Nome", nota.TipoId);
            return View(nota);
        }

        // GET: Nota/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Nota.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoId = new SelectList(db.Tipo, "TipoId", "Nome", nota.TipoId);
            return View(nota);
        }

        // POST: Nota/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NItemId,Descricao,CodItem,DescCompl,Ean,Qtd,VlItem,TipoId")] Nota nota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoId = new SelectList(db.Tipo, "TipoId", "Nome", nota.TipoId);
            return View(nota);
        }

        // GET: Nota/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nota nota = db.Nota.Find(id);
            if (nota == null)
            {
                return HttpNotFound();
            }
            return View(nota);
        }

        // POST: Nota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nota nota = db.Nota.Find(id);
            db.Nota.Remove(nota);
            db.SaveChanges();
            return RedirectToAction("Index");
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
