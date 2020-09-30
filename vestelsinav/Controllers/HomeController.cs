using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using vestelsinav.Models.Database_Models;
using vestelsinav.Models;
using System.Text;

namespace vestelsinav.Controllers
{
    public class HomeController : Controller
    {
        private db db = new db();

        private int sicilNo()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string stringSicil = ticket.Name;
            int id = int.Parse(stringSicil);

            return id;
        }

        [AllowAnonymous]
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("kayit")]
        public ActionResult KayitOl()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("kayit")]
        public ActionResult KayitOl(FormCollection form)
        {
            string s = form["email"];
            Kullanici existingEmail = (from a in db.Kullanici
                        select a)
                        .Where(x => x.email == s)
                        .FirstOrDefault();

            if (existingEmail != null)
            {
                ViewBag.emailHata = "Girdiğiniz e-mail zaten sistemde kayıtlı!";
                return View();
            }

            Kullanici k = new Kullanici();
            k.isim = form["isim"];
            k.soyisim = form["soyisim"];
            k.email = form["email"];
            k.sifre = form["sifre"];

            db.Kullanici.Add(k);
            db.SaveChanges();

            return RedirectToAction("Giris");
        }

        [AllowAnonymous]
        [Route("giris")]
        public ActionResult Giris()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("giris")]
        public ActionResult Giris(FormCollection form)
        {
            string s = form["sifre"], e = form["email"];
            Kullanici kullanici = (from a in db.Kullanici
                                        select a)
                                        .Where(x => x.sifre == s && x.email == e)
                                        .FirstOrDefault();

            if(kullanici != null)
            {
                FormsAuthentication.SetAuthCookie(kullanici.sicilNo.ToString(), false);
                return RedirectToAction("Sinavlar");
            }

            ViewBag.hataMesajı = "Hatalı sicil email/şifre!";
            return View();
        }

        [Route("egitimler")]
        public ActionResult Sinavlar()
        {
            if (Request.IsAuthenticated)
            {
                int id = sicilNo();

                var egitimListesi = (from a in db.Egitim
                                     select a).ToList();

                var kullanici = (from a in db.Kullanici
                                 select new
                                 {
                                     a.isim,
                                     a.soyisim,
                                     a.sicilNo
                                 })
                                     .Where(x => x.sicilNo == id)
                                     .First();

                ViewBag.isim = (kullanici.isim).ToUpper();
                ViewBag.soyisim = (kullanici.soyisim).ToUpper();

                return View(egitimListesi);
            }

            return RedirectToAction("/");
        }

        [Route("egitimler/onay")]
        public ActionResult Onay(int egitimID, int soruSayisi)
        {
            Egitim e = db.Egitim.Find(egitimID);
            ViewBag.egitimID = egitimID;
            ViewBag.egitimSuresi = e.egitimSuresi;

            if(soruSayisi == 0)
            {
                return RedirectToAction("Sinavlar");
            }
            return View();
        }

        [Route("egitim/soru")]
        public ActionResult Sorular(int? egitimID, SoruView gonderilenSoru)
        {
            if(egitimID != null)
            {
                int id = sicilNo();

                Egitim egitimObject = (from a in db.Egitim
                                       select a)
                                    .Where(x => x.egitimID == egitimID)
                                    .FirstOrDefault();

                int soruSayisi = egitimObject.soruSayisi;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < soruSayisi; i++)
                {
                    sb.Append(0);
                }

                Kullanici_Hareket kullaniciHareket = new Kullanici_Hareket();
                kullaniciHareket.baslamaTarihi = DateTime.Now;
                kullaniciHareket.egitimID = egitimID;
                kullaniciHareket.kullaniciID = id;
                kullaniciHareket.verdigiCevaplar = sb.ToString();

                db.Kullanici_Hareket.Add(kullaniciHareket);
                db.SaveChanges();
                
                SoruView soru = (from a in db.Soru
                                 select new vestelsinav.Models.SoruView
                                 {
                                     egitimID = a.egitimID,
                                     soruIcerigi = a.soruIcerigi,
                                     aSecenegi = a.aSecenegi,
                                     bSecenegi = a.bSecenegi,
                                     cSecenegi = a.cSecenegi,
                                     dSecenegi = a.dSecenegi,
                                     soruID = a.soruID,
                                     hareketID = kullaniciHareket.hareketID,
                                     kacinciSoru = 0
                                 })
                    .Where(x => x.egitimID == egitimID)
                    .FirstOrDefault();

                if(soruSayisi == 1)
                {
                    soru.sonMu = true;
                }

                return View(soru);
            } else
            {
                var obj = (SoruView) TempData["myObj"];
                gonderilenSoru = obj;
                return View(gonderilenSoru);
            }
        }

        [Route("egitim/soru")]
        [HttpPost]
        public ActionResult Sorular(PostModel model)
        {
            Kullanici_Hareket hareket = (from b in db.Kullanici_Hareket
                                         select b)
                                         .Where(x => x.hareketID == model.hareketID)
                                         .FirstOrDefault();

            var cevapArr = hareket.verdigiCevaplar.ToCharArray();

            cevapArr[model.kacinciSoru] = model.secenek;
            hareket.verdigiCevaplar = new string(cevapArr);

            var yeniSoru = model.ileriYon ? model.kacinciSoru + 1 : model.kacinciSoru - 1;

            SoruView soru = null;

            if (model.ileriYon)
            {
                soru = (from a in db.Soru
                               select new vestelsinav.Models.SoruView
                               {
                                   egitimID = a.egitimID,
                                   hareketID = hareket.hareketID,
                                   soruIcerigi = a.soruIcerigi,
                                   aSecenegi = a.aSecenegi,
                                   bSecenegi = a.bSecenegi,
                                   cSecenegi = a.cSecenegi,
                                   dSecenegi = a.dSecenegi,
                                   soruID = a.soruID,
                                   kacinciSoru = yeniSoru
                               })
                                .Where(x => x.egitimID == model.egitimID && x.soruID > model.soruID)
                                .OrderBy(x => x.soruID)
                                .FirstOrDefault();

            }
            else
            {
                soru = (from a in db.Soru
                               select new vestelsinav.Models.SoruView
                               {
                                   egitimID = a.egitimID,
                                   hareketID = hareket.hareketID,
                                   soruIcerigi = a.soruIcerigi,
                                   aSecenegi = a.aSecenegi,
                                   bSecenegi = a.bSecenegi,
                                   cSecenegi = a.cSecenegi,
                                   dSecenegi = a.dSecenegi,
                                   soruID = a.soruID,
                                   kacinciSoru = yeniSoru
                               })
                                        .Where(x => x.egitimID == model.egitimID && x.soruID < model.soruID)
                                        .OrderByDescending(x => x.soruID)
                                        .FirstOrDefault();
            }

            
            model.kacinciSoru = yeniSoru;
            db.SaveChanges();

            if(model.kacinciSoru + 1 == hareket.verdigiCevaplar.Length)
            {
                soru.sonMu = true;
            }
            if (model.isDone)
            {
                hareket.bitirmeTarihi = DateTime.Now;
                db.SaveChanges();

                return Json("FINISH");
            }
            var i = hareket.verdigiCevaplar[soru.kacinciSoru];
            if (i == 'A' || i == 'B' || i == 'C' || i == 'D')
            {
                soru.kayitliCevap = i;
            }
            var k = soru;
            TempData["myObj"] = k;
            return Json("OK");
        }

        [Route("kullanici/sonuclar")]
        public ActionResult KullaniciSonuc()
        {
            int id = sicilNo();
            List<Kullanici_Hareket> hareketList = (from a in db.Kullanici_Hareket
                                    select a)
                                    .Where(x => x.kullaniciID == id && x.bitirmeTarihi != null)
                                    .ToList();

            List<SingleResultList> srlList = new List<SingleResultList>();
            if(hareketList.Count == 0)
            {
                ViewBag.error = 1;

                return View();
            }

            ViewBag.error = 0;

            for (int i = 0; i < hareketList.Count; ++i)
            {
                int dogruSayisi = 0;
                int k = (int) hareketList.ElementAt(i).egitimID;
                Egitim e = (from b in db.Egitim
                            select b)
                            .Where(x => x.egitimID == k)
                            .FirstOrDefault();

                SingleResultList srl = new SingleResultList();
                srl.sinavBasligi = e.konuBasligi;
                srl.toplamSoruSayisi = e.soruSayisi;

                for (int j = 0; j < hareketList.ElementAt(i).verdigiCevaplar.Length; ++j)
                {
                    if (e.cevapAnahtari[j] == hareketList[i].verdigiCevaplar[j])
                       dogruSayisi++;
                }
                srl.dogruSayisi = dogruSayisi;
                srl.egitimID = e.egitimID;
                srlList.Add(srl);
            }
            return View(srlList);
        }

        [Route("kullanici/sonuclar/detay")]
        public ActionResult KullaniciSonucDetay(int egitimID)
        {
            int id = sicilNo();
            List<Soru> e = (from a in db.Soru
                            select a)
                        .Where(x => x.egitimID == egitimID)
                        .ToList();

            Kullanici k = (from b in db.Kullanici
                           select b)
                        .Where(x => x.sicilNo == id)
                        .FirstOrDefault();

            Kullanici_Hareket kH = (from c in db.Kullanici_Hareket
                                    select c)
                        .Where(x => x.egitimID == egitimID && x.kullaniciID == id && x.bitirmeTarihi != null)
                        .FirstOrDefault();

            ResultGetView rgv = new ResultGetView();
            rgv.k = k;
            rgv.soruList = e;
            rgv.verdigiCevaplar = kH.verdigiCevaplar;

            return View(rgv);
        }

        [Route("cikis")]
        public ActionResult Cikis()
        {
            var c = new HttpCookie("admin");
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(c);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
