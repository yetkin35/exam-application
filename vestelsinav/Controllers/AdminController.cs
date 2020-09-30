using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vestelsinav.Models.Database_Models;
using vestelsinav.Models;

namespace vestelsinav.Controllers
{
    public class AdminController : Controller
    {
        private db db = new db();

        [AllowAnonymous]
        [Route("yonetici-giris")]
        public ActionResult admingiris()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("yonetici-giris")]
        public ActionResult admingiris(FormCollection form)
        {
            int sicilNo = 0;
            string sifre = form["sifre"];
            Int32.TryParse(form["sicilNo"], out sicilNo);

            Admin admin = (from a in db.Admin
                               select a)
                                        .Where(x => x.sicilNo == sicilNo && x.sifre == sifre)
                                        .FirstOrDefault();

            if (admin != null)
            {
                FormsAuthentication.SetAuthCookie(admin.sicilNo.ToString(), false);
                HttpCookie cookie = new HttpCookie("admin");
                HttpContext.Response.Cookies.Remove("admin");
                HttpContext.Response.SetCookie(cookie);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.hataMesajı = "Hatalı sicil numarası.";

            return View();
        }

        [Route("admin-kullanici")]
        public ActionResult kullanici()
        {
            return View();
        }

        [Route("soru-duzenle")]
        public ActionResult soru()
        {
            List<Egitim> EgitimList = db.Egitim.ToList();
            ViewBag.EgitimList = new SelectList(EgitimList, "egitimID", "konuBasligi", "soruSayisi");
            return View();
        }
        [HttpPost]
        [Route("soru-duzenle")]
        public JsonResult soruDuzenle(int soruID)
        {

            SoruView soru = db.Soru.Where(x => x.soruID == soruID)
                                .Select(x => new SoruView()
                                {
                                    egitimID = x.egitimID,
                                    soruIcerigi = x.soruIcerigi,
                                    aSecenegi = x.aSecenegi,
                                    bSecenegi = x.bSecenegi,
                                    cSecenegi = x.cSecenegi,
                                    dSecenegi = x.dSecenegi,
                                    soruID = soruID,
                                    dogruCevap = x.dogruCevap
                                }).FirstOrDefault();


            return Json(soru, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("soru-duzenle-db")]
        public JsonResult soruDuzenleDb(SoruView soru, string a)
        {
            Soru s = new Soru();
            s = db.Soru.Find(soru.soruID);
            s.soruIcerigi = soru.soruIcerigi;
            s.aSecenegi = soru.aSecenegi;
            s.bSecenegi = soru.bSecenegi;
            s.cSecenegi = soru.cSecenegi;
            s.dSecenegi = soru.dSecenegi;
            s.dogruCevap = a;

            db.Entry(s).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json("ok");
        }

        [HttpPost]
        [Route("soru-ekle")]
        public ActionResult soruekle()
        {
            return View();
        }

        [HttpPost]
        [Route("soru-kaydet")]
        public ActionResult soruKaydet(Soru s)
        {
            Egitim e = db.Egitim.Find(s.egitimID);
            e.cevapAnahtari += s.dogruCevap;
            e.soruSayisi++;
            db.Soru.Add(s);
            db.SaveChanges();

            return Json("ok");
        }

        [HttpPost]
        [Route("sorulari-getir")]
        public JsonResult KonularaGoreSorulariGetir(int id)
        {
            List<SoruView> soruList = db.Soru.Where(x => x.egitimID == id)
                                .OrderBy(x => x.soruID)
                                .Select(x => new SoruView()
                                {
                                    soruID = x.soruID,
                                    egitimID = x.egitimID,
                                    soruIcerigi = x.soruIcerigi,
                                    aSecenegi = x.aSecenegi,
                                    bSecenegi = x.bSecenegi,
                                    cSecenegi = x.cSecenegi,
                                    dSecenegi = x.dSecenegi,
                                    dogruCevap = x.dogruCevap
                                }).ToList();

            if (soruList.Count == 0)
            {
                SoruView bosSonucDatasi = new SoruView();

                bosSonucDatasi.egitimID = id;

                soruList.Add(bosSonucDatasi);
            }
            return Json(soruList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("sinavlari-getir")]
        public JsonResult KonularaGoreSinavlariGetir(int id)
        {
            Egitim e = db.Egitim.Find(id);
            List<Kullanici_Hareket> hareketList = (from a in db.Kullanici_Hareket
                                                   select a)
                                                   .Where(x => x.egitimID == id && x.bitirmeTarihi != null)
                                                   .ToList();

            List<ResultView> r = new List<ResultView>();

            for (int i = 0; i < hareketList.Count; i++)
            {
                var mm = hareketList[i].kullaniciID;
                Kullanici k = (from b in db.Kullanici
                               select b)
                               .Where(x => x.sicilNo == mm)
                               .FirstOrDefault();
                if(k != null)
                {
                    TimeSpan span = (hareketList[i].bitirmeTarihi - hareketList[i].baslamaTarihi).Value;
                    int dogruSayisi = 0;

                    for (int j = 0; j < hareketList[i].verdigiCevaplar.Length; j++)
                    {
                        if(e.cevapAnahtari[j] == hareketList[i].verdigiCevaplar[j])
                            dogruSayisi++;
                    }

                    ResultView specificResult = new ResultView();
                    specificResult.sicilNo = k.sicilNo;
                    specificResult.verdigiCevaplar = hareketList[i].verdigiCevaplar;
                    specificResult.bitirmeSuresi = span.TotalSeconds;
                    specificResult.isim = k.isim;
                    specificResult.soyisim = k.soyisim;
                    specificResult.dogruSayisi = dogruSayisi;
                    specificResult.toplamSoruSayisi = e.soruSayisi;
                    specificResult.egitimID = id;
                    r.Add(specificResult);
                }
            }

            return Json(r, JsonRequestBehavior.AllowGet);
        }

        [Route("sonuc-detay")]
        public ActionResult SonucDetay(int sicilNo, int egitimID)
        {
            List<Soru> e = (from a in db.Soru
                        select a)
                        .Where(x => x.egitimID == egitimID)
                        .ToList();

            Kullanici k = (from b in db.Kullanici
                           select b)
                        .Where(x => x.sicilNo == sicilNo)
                        .FirstOrDefault();

            Kullanici_Hareket kH = (from c in db.Kullanici_Hareket
                           select c)
                        .Where(x => x.egitimID == egitimID && x.kullaniciID == sicilNo)
                        .FirstOrDefault();

            ResultGetView rgv = new ResultGetView();
            rgv.k = k;
            rgv.soruList = e;
            rgv.verdigiCevaplar = kH.verdigiCevaplar;

            return View(rgv);
        }
        
        [Route("sinav/sinav-ekle")]
        [HttpPost]
        public ActionResult Sorular(int soruID)
        {
            Soru s = (from a in db.Soru select a)
                .Where(x => x.soruID == soruID)
                .FirstOrDefault();
            if (s != null)
            {
                db.Soru.Remove(s);
                db.SaveChanges();
                return Json("ok");
            }
            else
            {
                return Json("error.!");
            }

        }

        [HttpPost]
        [Route("sinav-kaydet")]
        public JsonResult konuKaydet(Egitim a)
        {   
            if(a != null)
            {
                db.Egitim.Add(a);
                db.SaveChanges();
            }

            return Json("ok");
        }
        
        [HttpPost]
        [Route("sinav-duzenle")]
        public JsonResult konuDuzenle(int egitimID)
        {
            // sınavın icinde soru var ise 500error veriyor. database foreign key
            Egitim egitim = (from a in db.Egitim
                             select a)
                             .Where(x => x.egitimID == egitimID)
                             .FirstOrDefault();

            Egitim e = new Egitim();
            e.konuBasligi = egitim.konuBasligi;
            e.egitimSuresi = egitim.egitimSuresi;

            return Json(e, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("sinav-duzenle-db")]
        public JsonResult konuDuzenleDb(Egitim egitim)
        {
            Egitim s = new Egitim();
            s = db.Egitim.Find(egitim.egitimID);
            s.cevapAnahtari = egitim.cevapAnahtari;
            s.konuBasligi = egitim.konuBasligi;
            s.egitimSuresi = egitim.egitimSuresi;

            db.Entry(s).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json("ok");
        }

        [Route("sinav-liste")]
        public ActionResult konu()
        {
            var egitimListesi = (from a in db.Egitim
                                 select a).ToList();
            return View(egitimListesi);
        }

        [Route("sinav/sinav-sil")]
        [HttpPost]
        public ActionResult konusil(int egitimID)
        {
            Egitim s = (from a in db.Egitim select a)
                .Where(x => x.egitimID == egitimID)
                .FirstOrDefault();
            
            foreach (Kullanici_Hareket silinecekHareket in s.Kullanici_Hareket.ToList())
                db.Kullanici_Hareket.Remove(silinecekHareket);
            
            foreach (Soru silinecekSoru in s.Soru.ToList())
                db.Soru.Remove(silinecekSoru);

            if (s != null)
            {
                db.Egitim.Remove(s);
                db.SaveChanges();
                return Json("ok");
            }
            else
            {
                return Json("error.!");
            }
        }

        [Route("sonuclar")]
        public ActionResult SonucAl()
        {
            List<Egitim> EgitimList = db.Egitim.ToList();
            ViewBag.EgitimList = new SelectList(EgitimList, "egitimID", "konuBasligi", "soruSayisi");
            return View();
        }
    }
}
