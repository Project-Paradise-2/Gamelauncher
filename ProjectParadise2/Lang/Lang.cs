using ProjectParadise2.Core.Log;
using System;

namespace ProjectParadise2
{
    internal class Lang
    {
        public static string GetAboutText()
        {
            string message = GetAboutText(Database.Database.p2Database.Usersettings.LangId);

            if (CommandLineArg.DebugUI)
                message += $" [ 0 | {Database.Database.p2Database.Usersettings.LangId} ]";

            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            else
            {
                return $"Missing Translation, {Database.Database.p2Database.Usersettings.LangId}";
            }
        }

        public static string GetAboutText(int LangId)
        {
            try
            {
                string aboutText;
                switch (LangId)
                {
                    case 0: // German
                        aboutText = "Project Paradise 2 ist ein Community-Server-Emulator für Test Drive Unlimited 2. Es stellt die Online-Funktionen wieder her und bietet neue Community-Features.\n\nFeatures:\n- Stabile Server\n- Aktive Community\n- Neue Community-Features\n- Regelmäßige Updates\n- Fanprojekt, nicht offiziell\n\nHinweise:\n- Dieses Projekt steht in keiner Verbindung zu Atari oder Eden Games.\n- Reverse Engineering oder Modifikationen am Client sind untersagt.\n- Cheating oder Exploits werden nicht toleriert.\n\nServerfunktionen (Stand 17/07/2025):\n\nFunktionierende Funktionen:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Wetter-System: Originalsystem wiederhergestellt, Regenrate reduziert\n- Clubs: Erstellen, Beitreten, Clubinterne Rennen, Autos freischalten und Upgrades\n- Ranglisten: Einzelspieler-Rennen, Casino, Ranglisten\n- Freunde: Suchen, Hinzufügen, Entfernen\n- Casino: Slots, Roulette\n\nDerzeit nicht funktionierende Funktionen:\n- Casino: Join/Invite, Turnierspiele\n- Clubs: Club vs Club Rennen\n";
                        break;

                    case 1: // English
                    case 11: // English_UK
                        aboutText = "Project Paradise 2 is a community server emulator for Test Drive Unlimited 2. It restores online functionality and adds new community-driven features.\n\nFeatures:\n- Server Stability\n- Active Community\n- New Community Features\n- Regular Updates\n- Fan project, not official\n\nNotes:\n- This project is not affiliated with Atari or Eden Games.\n- Reverse engineering or client modifications are prohibited.\n- Cheating or exploits are not tolerated.\n\nServer Functions (Info as of 17/07/2025):\n\nCurrently Functional:\n- Lobbies: Freeroam, Races (Ranked, Unranked), House Lobbies\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Restored original system, rain rate reduced per community request\n- Clubs: Creating, Joining, Club-internal Races, Unlocking Cars and Upgrade Clubs\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nCurrently Not Functional:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 2: // Czech
                        aboutText = "Project Paradise 2 je komunitní serverový emulátor pro Test Drive Unlimited 2. Obnovuje online funkce a přidává nové prvky od komunity.\n\nFunkce:\n- Stabilní servery\n- Aktivní komunita\n- Nové komunitní funkce\n- Pravidelné aktualizace\n- Fanouškovský projekt, neoficiální\n\nUpozornění:\n- Tento projekt není spojen s Atari ani Eden Games.\n- Reverzní inženýrství nebo úpravy klienta jsou zakázány.\n- Podvádění nebo zneužití nejsou tolerovány.\n\nServerové funkce (k 17/07/2025):\n\nFunkční:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Původní systém obnoven, snížena intenzita deště dle komunity\n- Clubs: Vytváření, Připojování, Klubová závody, Odemykání aut a upgrade klubů\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Hledat, Přidat, Odebrat\n- Casino: Slots, Roulette\n\nMomentálně nefunkční:\n- Casino: Join/Invite, Turnament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 3: // Spanish
                        aboutText = "Project Paradise 2 es un emulador de servidor comunitario para Test Drive Unlimited 2. Restaura las funciones en línea y añade nuevas características creadas por la comunidad.\n\nFunciones:\n- Servidores estables\n- Comunidad activa\n- Nuevas características comunitarias\n- Actualizaciones regulares\n- Proyecto de fans, no oficial\n\nAvisos:\n- Este proyecto no está afiliado con Atari ni Eden Games.\n- Ingeniería inversa o modificaciones del cliente están prohibidas.\n- No se toleran trampas ni exploits.\n\nFunciones del servidor (17/07/2025):\n\nActualmente funcionales:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Sistema original restaurado, lluvia reducida según petición de la comunidad\n- Clubs: Creación, Unirse, Carreras internas del club, Desbloqueo de autos y mejoras del club\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Buscar, Añadir, Eliminar\n- Casino: Slots, Roulette\n\nActualmente no funcionales:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 4: // French
                        aboutText = "Project Paradise 2 est un émulateur de serveur communautaire pour Test Drive Unlimited 2. Il restaure les fonctionnalités en ligne et ajoute de nouvelles options créées par la communauté.\n\nFonctionnalités:\n- Serveurs stables\n- Communauté active\n- Nouvelles fonctionnalités communautaires\n- Mises à jour régulières\n- Projet de fans, non officiel\n\nRemarques:\n- Ce projet n'est affilié à Atari ou Eden Games.\n- Le reverse engineering ou les modifications du client sont interdits.\n- Les triches ou exploits ne sont pas tolérés.\n\nFonctions du serveur (17/07/2025):\n\nFonctionnelles:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Système original restauré, pluie réduite selon demande de la communauté\n- Clubs: Création, Rejoindre, Courses internes du club, Déblocage des voitures et améliorations du club\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Rechercher, Ajouter, Supprimer\n- Casino: Slots, Roulette\n\nActuellement non fonctionnelles:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 5: // Indonesian
                        aboutText = "Project Paradise 2 adalah emulator server komunitas untuk Test Drive Unlimited 2. Proyek ini mengembalikan fitur online dan menambahkan fitur baru dari komunitas.\n\nFitur:\n- Server stabil\n- Komunitas aktif\n- Fitur komunitas baru\n- Pembaruan rutin\n- Proyek penggemar, tidak resmi\n\nCatatan:\n- Proyek ini tidak berafiliasi dengan Atari atau Eden Games.\n- Reverse engineering atau modifikasi client dilarang.\n- Cheating atau exploit tidak ditoleransi.\n\nFungsi Server (Info per 17/07/2025):\n\nSaat ini berfungsi:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Sistem asli dipulihkan, tingkat hujan dikurangi sesuai permintaan komunitas\n- Clubs: Membuat, Bergabung, Balapan internal klub, Membuka mobil dan Upgrade Clubs\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nSaat ini tidak berfungsi:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 6: // Italian
                        aboutText = "Project Paradise 2 è un emulatore di server comunitario per Test Drive Unlimited 2. Ripristina le funzionalità online e aggiunge nuove caratteristiche create dalla community.\n\nCaratteristiche:\n- Server stabili\n- Community attiva\n- Nuove caratteristiche della community\n- Aggiornamenti regolari\n- Progetto dei fan, non ufficiale\n\nAvvertenze:\n- Questo progetto non è affiliato con Atari o Eden Games.\n- Reverse engineering o modifiche del client sono vietate.\n- Cheat o exploit non sono tollerati.\n\nFunzioni del server (Info al 17/07/2025):\n\nFunzionanti:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Sistema originale ripristinato, pioggia ridotta su richiesta della community\n- Clubs: Creazione, Unirsi, Gare interne del club, Sbloccare auto e upgrade club\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nAttualmente non funzionanti:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 7: // Lithuanian
                        aboutText = "Project Paradise 2 yra bendruomenės serverio emuliatorius Test Drive Unlimited 2 žaidimui. Jis atkuria internetines funkcijas ir prideda naujų bendruomenės sukurtų galimybių.\n\nFunkcijos:\n- Stabilių serverių\n- Aktyvi bendruomenė\n- Naujos bendruomenės funkcijos\n- Reguliarūs atnaujinimai\n- Fanų projektas, neoficialus\n\nPastabos:\n- Šis projektas nėra susijęs su Atari ar Eden Games.\n- Reverse engineering arba kliento modifikacijos draudžiamos.\n- Cheatinimas ar exploit'ai netoleruojami.\n\nServerio funkcijos (2025-07-17):\n\nVeikiančios:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Originalus sistema atkurta, lietaus dažnis sumažintas pagal bendruomenės pageidavimą\n- Clubs: Kūrimas, Prisijungimas, Klubinės lenktynės, Automobilių atrakimas ir klubų patobulinimai\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nŠiuo metu neveikia:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 8: // Polish
                        aboutText = "Project Paradise 2 to emulator serwera społecznościowego dla Test Drive Unlimited 2. Przywraca funkcje online i dodaje nowe funkcje tworzone przez społeczność.\n\nFunkcje:\n- Stabilne serwery\n- Aktywna społeczność\n- Nowe funkcje społeczności\n- Regularne aktualizacje\n- Projekt fanowski, nieoficjalny\n\nUwagi:\n- Projekt nie jest powiązany z Atari ani Eden Games.\n- Reverse engineering lub modyfikacje klienta są zabronione.\n- Cheaty lub exploity nie są tolerowane.\n\nFunkcje serwera (17/07/2025):\n\nDziałające:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Przywrócono oryginalny system, zmniejszono deszcz według życzeń społeczności\n- Clubs: Tworzenie, Dołączanie, Wyścigi wewnętrzne klubów, Odblokowywanie aut i ulepszenia klubów\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nObecnie nie działające:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 9: // Russian
                        aboutText = "Project Paradise 2 — это эмулятор серверов сообщества для Test Drive Unlimited 2. Он восстанавливает сетевые функции и добавляет новые возможности от сообщества.\n\nФункции:\n- Стабильные серверы\n- Активное сообщество\n- Новые функции сообщества\n- Регулярные обновления\n- Фан-проект, неофициально\n\nПримечания:\n- Этот проект не связан с Atari или Eden Games.\n- Reverse engineering или модификации клиента запрещены.\n- Читы и эксплойты не допускаются.\n\nФункции сервера (17/07/2025):\n\nРаботают:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Восстановлена оригинальная система, дождь уменьшен по просьбе сообщества\n- Clubs: Создание, Присоединение, Внутриклубные гонки, Разблокировка машин и улучшение клубов\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nВ настоящее время не работают:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 10: // Portuguese_Brazil
                        aboutText = "Project Paradise 2 é um emulador de servidor da comunidade para Test Drive Unlimited 2. Ele restaura as funções online e adiciona novos recursos feitos pela comunidade.\n\nRecursos:\n- Servidores estáveis\n- Comunidade ativa\n- Novos recursos da comunidade\n- Atualizações regulares\n- Projeto de fãs, não oficial\n\nAvisos:\n- Este projeto não é afiliado à Atari ou Eden Games.\n- Engenharia reversa ou modificações no cliente são proibidas.\n- Cheats ou exploits não são tolerados.\n\nFunções do servidor (Info em 17/07/2025):\n\nAtualmente funcionando:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Sistema original restaurado, taxa de chuva reduzida por solicitação da comunidade\n- Clubs: Criar, Entrar, Corridas internas do clube, Desbloqueio de carros e upgrades do clube\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nAtualmente não funcionando:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 12: // Dutch
                        aboutText = "Project Paradise 2 is een community-serveremulator voor Test Drive Unlimited 2. Het herstelt de online functies en voegt nieuwe functies toe die door de community zijn gemaakt.\n\nFuncties:\n- Stabiele servers\n- Actieve community\n- Nieuwe community-functies\n- Regelmatige updates\n- Fanproject, niet officieel\n\nOpmerkingen:\n- Dit project is niet gelieerd aan Atari of Eden Games.\n- Reverse engineering of clientwijzigingen zijn verboden.\n- Cheating of exploits worden niet getolereerd.\n\nServerfuncties (17/07/2025):\n\nMomenteel functioneel:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Origineel systeem hersteld, regen verminderd op verzoek van de community\n- Clubs: Creëren, Lid worden, Clubinterne races, Auto’s ontgrendelen en club upgrades\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nMomenteel niet functioneel:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 13: // Romanian
                        aboutText = "Project Paradise 2 este un emulator de server comunitar pentru Test Drive Unlimited 2. Restaurează funcțiile online și adaugă caracteristici noi create de comunitate.\n\nFuncționalități:\n- Servere stabile\n- Comunitate activă\n- Funcționalități noi ale comunității\n- Actualizări regulate\n- Proiect de fani, neoficial\n\nNotă:\n- Acest proiect nu este afiliat cu Atari sau Eden Games.\n- Reverse engineering sau modificări ale clientului sunt interzise.\n- Cheat-uri sau exploatări nu sunt tolerate.\n\nFuncții server (Info la 17/07/2025):\n\nFuncționale:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Sistemul original restaurat, rata ploii redusă la cererea comunității\n- Clubs: Creare, Alăturare, Curse interne club, Deblocare mașini și upgrade club\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nÎn prezent nefuncționale:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 14: // Turkish
                        aboutText = "Project Paradise 2, Test Drive Unlimited 2 için bir topluluk sunucu emülatörüdür. Çevrimiçi işlevleri geri getirir ve topluluk tarafından oluşturulan yeni özellikler ekler.\n\nÖzellikler:\n- Kararlı sunucular\n- Aktif topluluk\n- Yeni topluluk özellikleri\n- Düzenli güncellemeler\n- Hayran projesi, resmi değil\n\nNotlar:\n- Bu proje Atari veya Eden Games ile bağlantılı değildir.\n- Reverse engineering veya istemci değişiklikleri yasaktır.\n- Hile veya exploit kabul edilmez.\n\nSunucu Fonksiyonları (17/07/2025):\n\nŞu anda çalışanlar:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Orijinal sistem geri yüklendi, yağmur oranı topluluk isteğine göre azaltıldı\n- Clubs: Oluşturma, Katılma, Kulüp içi yarışlar, Arabaların kilidini açma ve kulüp geliştirmeleri\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nŞu anda çalışmayanlar:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 15: // Chinese
                        aboutText = "Project Paradise 2 是《无限试驾2》的社区服务器模拟器。它恢复了在线功能，并加入了新的社区功能。\n\n功能:\n- 稳定的服务器\n- 活跃的社区\n- 新的社区功能\n- 定期更新\n- 粉丝项目，非官方\n\n注意事项:\n- 本项目与 Atari 或 Eden Games 无关。\n- 禁止对客户端进行反向工程或修改。\n- 禁止作弊或利用漏洞。\n\n服务器功能 (2025/07/17):\n\n目前功能正常:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbys\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: 恢复原始系统，雨量根据社区要求降低\n- Clubs: 创建, 加入, 俱乐部内部赛事, 解锁车辆及俱乐部升级\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\n目前功能异常:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;

                    case 16: // Debug
                        aboutText = "Debug Language ID: " + LangId + "\n";
                        break;

                    default: // Fallback
                        aboutText = "Project Paradise 2 is a community server emulator for Test Drive Unlimited 2. It restores online functionality and adds new community-driven features.\n\nFeatures:\n- Server Stability\n- Active Community\n- New Community Features\n- Regular Updates\n- Fan project, not official\n\nNotes:\n- This project is not affiliated with Atari or Eden Games.\n- Reverse engineering or client modifications are prohibited.\n- Cheating or exploits are not tolerated.\n\nServer Functions (Info as of 17/07/2025):\n\nCurrently Functional:\n- Lobbys: Freeroam, Races (Ranked, Unranked), House Lobbies\n- Stats: TDUMyLife, Casino, Playerstats\n- Community Racing Center: CRC, ORC\n- Weather System: Restored original system, rain rate reduced per community request\n- Clubs: Creating, Joining, Club-internal Races, Unlocking Cars and Upgrade Clubs\n- Leaderboards: Singleplayer Races, Casino, Ranking lists\n- Friends: Search, Add and Remove\n- Casino: Slots, Roulette\n\nCurrently Not Functional:\n- Casino: Join/Invite, Tournament Game\n- Clubs: Club vs Club Races\n";
                        break;
                }

                return aboutText + "\n"; // finaler Zeilenumbruch
            }
            catch (Exception ex)
            {
                Log.Error($"Missing Translation Text: 0 Lang: {LangId}", ex);
                return string.Empty;
            }
        }

        public static string GetTooltipText(int TextId)
        {
            string message = GetTooltipText(TextId, Database.Database.p2Database.Usersettings.LangId);

            if (CommandLineArg.DebugUI)
                message += $" [ {TextId} | {Database.Database.p2Database.Usersettings.LangId} ]";

            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            else
            {
                return $"Missing Translation, {TextId}:{Database.Database.p2Database.Usersettings.LangId}";
            }
        }

        public static string GetTooltipText(int TextId, int LangId)
        {
            try
            {
                switch (LangId)
                {
                    case 0: //German
                        return GermanToolTip[TextId];
                    case 1: //English
                        return EnglishToolTip[TextId];
                    case 2: //Czech
                        return CzechToolTip[TextId];
                    case 3: //Spanish
                        return SpanishToolTip[TextId];
                    case 4: //French
                        return FrenchToolTip[TextId];
                    case 5: //Indonesian
                        return IndonesianToolTip[TextId];
                    case 6: //Italian
                        return ItalianToolTip[TextId];
                    case 7: //Lithuanian
                        return LithuanianToolTip[TextId];
                    case 8: //Polish
                        return PolishToolTip[TextId];
                    case 9: //Russian
                        return RussianToolTip[TextId];

                    case 10: //Portuguese_Brazil
                        return Portuguese_BrazilToolTip[TextId];
                    case 11: //English_UK
                        return English_UKToolTip[TextId];
                    case 12: //Dutch
                        return DutchToolTip[TextId];
                    case 13: //Romanian
                        return RomanianToolTip[TextId];
                    case 14: //Turkish
                        return TurkishToolTip[TextId];
                    case 15: //Chinese
                        return ChineseToolTip[TextId];
                    case 16: //Debug
                        return TextId + " | " + LangId;
                }
                return English[TextId];
            }
            catch (Exception ex)
            {
                Log.Error($"Missing Translation Text: {TextId} Lang: {LangId}", ex);
                return string.Empty;
            }
        }

        public static string GetText(int TextId)
        {
            string message = GetText(TextId, Database.Database.p2Database.Usersettings.LangId);

            if (CommandLineArg.DebugUI)
                message += $" [ {TextId} | {Database.Database.p2Database.Usersettings.LangId} ]";

            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            else
            {
                return $"Missing Translation, {TextId}:{Database.Database.p2Database.Usersettings.LangId}";
            }
        }

        public static string GetText(int TextId, int LangId)
        {
            try
            {
                switch (LangId)
                {
                    case 0: //German
                        return German[TextId];
                    case 1: //English
                        return English[TextId];
                    case 2: //Czech
                        return Czech[TextId];
                    case 3: //Spanish
                        return Spanish[TextId];
                    case 4: //French
                        return French[TextId];
                    case 5: //Indonesian
                        return Indonesian[TextId];
                    case 6: //Italian
                        return Italian[TextId];
                    case 7: //Lithuanian
                        return Lithuanian[TextId];
                    case 8: //Polish
                        return Polish[TextId];
                    case 9: //Russian
                        return Russian[TextId];

                    case 10: //Portuguese_Brazil
                        return Portuguese_Brazil[TextId];
                    case 11: //English_UK
                        return English_UK[TextId];
                    case 12: //Dutch
                        return Dutch[TextId];
                    case 13: //Romanian
                        return Romanian[TextId];
                    case 14: //Turkish
                        return Turkish[TextId];
                    case 15: //Chinese
                        return Chinese[TextId];
                    case 16: //Debug
                        return TextId + " | " + LangId;
                }
                return English[TextId];
            }
            catch (Exception ex)
            {
                Log.Error($"Missing Translation Text: {TextId} Lang: {LangId}", ex);
                return string.Empty;
            }
        }

        public static string[] GermanToolTip =
        {
            "Discord-Spielintegration\nDeaktiv | Aktiv – Zeigt oder versteckt den Status",                                                                 //0
            "Launcher-Update-Prüfung\nDeaktiv | Aktiv – Prüft auf neue Launcher-Versionen",                                                              //1
            "Universal Plug and Play\nDeaktiv | Aktiv – Versucht, die Portweiterleitung automatisch einzurichten",                                      //2
            "Sprache, die der Launcher verwenden soll",                                                                                                 //3
            "Audio-Modus: Standard oder Erweitert\nTDU2 hat zwei Audio-Modi, hier kann gewechselt werden",                                              //4
            "Erstellt ein neues Spielprofil",                                                                                                           //5
            "Dynamischer RAM\nDeaktiv | Aktiv – Erlaubt der Anwendung, mehr RAM zu nutzen",                                                            //6
            "Dynamische Kerne\nDeaktiv | Aktiv – Versucht, mehr CPU-Kerne zuzuweisen",                                                                 //7
            "Dynamische Priorität\nDeaktiv | Aktiv – Setzt den Prozess höher als normal",                                                             //8
            "Fahrzeugdreck\nDeaktiv | Aktiv – Fahrzeuge können verschmutzen",                                                                          //9
            "Fahrzeugschaden\nDeaktiv | Aktiv – Fahrzeuge können Schaden nehmen",                                                                      //10
            "Online-Modus\nDeaktiv | Aktiv – Startet das Spiel im Online- oder Offline-Modus",                                                        //11
            "Wählt das Spiel aus, das genutzt werden soll",                                                                                            //12
            "Öffnet den Spieleordner",                                                                                                                  //13
            "Spielt-Update\nPrüft auf fehlende oder beschädigte Dateien",                                                                              //14
            "Ändert die Spielkonfiguration,\num das Spiel mit dem PP2-Server zu verbinden.\nSollte genutzt werden, wenn Hostfile-Edit nicht funktioniert", //15
            "Öffnet den Mod-Browser, um Mods in dieser Spielinstanz zu verwalten",                                                                     //16
            "Profil speichern",                                                                                                                         //17
            "In Cloud einloggen",                                                                                                                       //18
            "Von Cloud ausloggen",                                                                                                                      //19
            "Daten vom Server aktualisieren",                                                                                                          //20
            "Lokales Backup erstellen\nDeaktiv | Aktiv",                                                                                               //21
            "Backup-Ordner öffnen",                                                                                                                     //22
            "Savegame-Backup erstellen vor oder nach Spielstart/-ende",                                                                                //23
            "Maximale Anzahl an Savegames, die aufgehoben werden sollen",                                                                              //24
            "Savegame-Ordner öffnen",                                                                                                                   //25
            "Savegame aus der Cloud herunterladen",                                                                                                     //26
            "Savegame in die Cloud hochladen",                                                                                                         //27
        };

        public static string[] EnglishToolTip =
        {
            "Discord game integration\nOff | On – Shows or hides the status",                                                                  //0
            "Launcher update check\nOff | On – Checks for new launcher versions",                                                              //1
            "Universal Plug and Play\nOff | On – Attempts to set up port forwarding automatically",                                           //2
            "Language the launcher should use",                                                                                                 //3
            "Audio mode: Standard or Advanced\nTDU2 has two audio modes, you can switch between them",                                         //4
            "Create a new game profile",                                                                                                        //5
            "Dynamic RAM\nOff | On – Allows the application to use more RAM",                                                                  //6
            "Dynamic cores\nOff | On – Attempts to assign more CPU cores",                                                                     //7
            "Dynamic priority\nOff | On – Sets the process to a higher priority than normal",                                                  //8
            "Vehicle dirt\nOff | On – Vehicles can get dirty",                                                                                 //9
            "Vehicle damage\nOff | On – Vehicles can take damage",                                                                             //10
            "Online mode\nOff | On – Starts the game in online or offline mode",                                                              //11
            "Select the game to be used",                                                                                                       //12
            "Open the game folder",                                                                                                             //13
            "Game update\nChecks for missing or corrupted files",                                                                              //14
            "Changes the game configuration\nto connect the game to the PP2 server.\nShould be used if Hostfile-Edit does not work",          //15
            "Opens the mod browser to manage mods in this game instance",                                                                      //16
            "Save profile",                                                                                                                     //17
            "Log in to cloud",                                                                                                                  //18
            "Log out of cloud",                                                                                                                 //19
            "Refresh server data",                                                                                                              //20
            "Create local backup\nOff | On",                                                                                                    //21
            "Open backup folder",                                                                                                               //22
            "Create savegame backup before or after game start/end",                                                                           //23
            "Maximum number of savegames to keep",                                                                                             //24
            "Open savegame folder",                                                                                                             //25
            "Download savegame from cloud",                                                                                                     //26
            "Upload savegame to cloud",                                                                                                         //27
        };

        public static string[] CzechToolTip =
        {
            "Integrace hry s Discordem\nVypnuto | Zapnuto – Zobrazuje nebo skrývá stav",                                                    //0
            "Kontrola aktualizace spouštěče\nVypnuto | Zapnuto – Kontroluje nové verze spouštěče",                                         //1
            "Universal Plug and Play\nVypnuto | Zapnuto – Pokusí se automaticky nastavit přesměrování portů",                             //2
            "Jazyk, který má spouštěč používat",                                                                                        //3
            "Zvukový režim: Standardní nebo Pokročilý\nTDU2 má dva zvukové režimy, můžete mezi nimi přepínat",                              //4
            "Vytvořit nový herní profil",                                                                                                //5
            "Dynamická RAM\nVypnuto | Zapnuto – Umožňuje aplikaci použít více paměti RAM",                                                //6
            "Dynamická jádra\nVypnuto | Zapnuto – Pokusí se přiřadit více CPU jader",                                                      //7
            "Dynamická priorita\nVypnuto | Zapnuto – Nastaví proces na vyšší prioritu než obvykle",                                       //8
            "Špína vozidel\nVypnuto | Zapnuto – Vozidla mohou být špinavá",                                                              //9
            "Poškození vozidel\nVypnuto | Zapnuto – Vozidla mohou být poškozena",                                                         //10
            "Online režim\nVypnuto | Zapnuto – Spustí hru v online nebo offline režimu",                                                  //11
            "Vyberte hru, která se má použít",                                                                                           //12
            "Otevřít složku hry",                                                                                                        //13
            "Aktualizace hry\nKontroluje chybějící nebo poškozené soubory",                                                              //14
            "Změní konfiguraci hry\na propojí hru se serverem PP2.\nMělo by se použít, pokud úprava hostitelského souboru nefunguje",        //15
            "Otevře prohlížeč modů pro správu modů v této instanci hry",                                                                 //16
            "Uložit profil",                                                                                                             //17
            "Přihlásit se do cloudu",                                                                                                    //18
            "Odhlásit se z cloudu",                                                                                                      //19
            "Obnovit data serveru",                                                                                                      //20
            "Vytvořit místní zálohu\nVypnuto | Zapnuto",                                                                                  //21
            "Otevřít složku záloh",                                                                                                      //22
            "Vytvořit zálohu uložené hry před nebo po spuštění/ukončení hry",                                                             //23
            "Maximální počet uložených her, které mají být uchovány",                                                                    //24
            "Otevřít složku uložených her",                                                                                              //25
            "Stáhnout uloženou hru z cloudu",                                                                                            //26
            "Nahrát uloženou hru do cloudu",
        };

        public static string[] SpanishToolTip =
        {
            "Integración del juego con Discord\nOff | On – Muestra u oculta el estado",                                                    //0
            "Comprobación de actualización del lanzador\nOff | On – Busca nuevas versiones del lanzador",                                   //1
            "Universal Plug and Play\nOff | On – Intenta configurar el reenvío de puertos automáticamente",                                //2
            "Idioma que debe usar el lanzador",                                                                                            //3
            "Modo de audio: Estándar o Avanzado\nTDU2 tiene dos modos de audio, puedes cambiar entre ellos",                                //4
            "Crear un nuevo perfil de juego",                                                                                              //5
            "RAM dinámica\nOff | On – Permite que la aplicación use más memoria RAM",                                                       //6
            "Núcleos dinámicos\nOff | On – Intenta asignar más núcleos de CPU",                                                             //7
            "Prioridad dinámica\nOff | On – Establece el proceso a una prioridad superior a la normal",                                     //8
            "Suciedad del vehículo\nOff | On – Los vehículos pueden ensuciarse",                                                           //9
            "Daño del vehículo\nOff | On – Los vehículos pueden sufrir daños",                                                             //10
            "Modo en línea\nOff | On – Inicia el juego en modo online o offline",                                                          //11
            "Selecciona el juego que se usará",                                                                                            //12
            "Abrir carpeta del juego",                                                                                                      //13
            "Actualización del juego\nComprueba archivos faltantes o corruptos",                                                           //14
            "Cambia la configuración del juego\npara conectar el juego al servidor PP2.\nDebe usarse si la edición del archivo host no funciona", //15
            "Abre el navegador de mods para gestionar mods en esta instancia del juego",                                                   //16
            "Guardar perfil",                                                                                                               //17
            "Iniciar sesión en la nube",                                                                                                    //18
            "Cerrar sesión en la nube",                                                                                                     //19
            "Actualizar datos del servidor",                                                                                                //20
            "Crear copia de seguridad local\nOff | On",                                                                                     //21
            "Abrir carpeta de copias de seguridad",                                                                                         //22
            "Crear copia de seguridad del juego antes o después de iniciar/cerrar",                                                        //23
            "Número máximo de partidas guardadas a conservar",                                                                              //24
            "Abrir carpeta de partidas guardadas",                                                                                          //25
            "Descargar partida guardada desde la nube",                                                                                    //26
            "Subir partida guardada a la nube",
        };

        public static string[] FrenchToolTip =
        {
            "Intégration du jeu avec Discord\nOff | On – Affiche ou masque le statut",                                                    //0
            "Vérification de mise à jour du lanceur\nOff | On – Recherche de nouvelles versions du lanceur",                               //1
            "Plug and Play universel\nOff | On – Tente de configurer automatiquement le redirection de port",                              //2
            "Langue que doit utiliser le lanceur",                                                                                        //3
            "Mode audio : Standard ou Avancé\nTDU2 possède deux modes audio, vous pouvez basculer entre eux",                               //4
            "Créer un nouveau profil de jeu",                                                                                             //5
            "RAM dynamique\nOff | On – Permet à l’application d’utiliser plus de mémoire RAM",                                             //6
            "Cœurs dynamiques\nOff | On – Tente d’assigner plus de cœurs CPU",                                                            //7
            "Priorité dynamique\nOff | On – Définit le processus à une priorité plus élevée que la normale",                               //8
            "Saleté des véhicules\nOff | On – Les véhicules peuvent se salir",                                                            //9
            "Dommages des véhicules\nOff | On – Les véhicules peuvent subir des dommages",                                                //10
            "Mode en ligne\nOff | On – Lance le jeu en mode en ligne ou hors ligne",                                                      //11
            "Sélectionner le jeu à utiliser",                                                                                             //12
            "Ouvrir le dossier du jeu",                                                                                                    //13
            "Mise à jour du jeu\nVérifie les fichiers manquants ou corrompus",                                                            //14
            "Modifie la configuration du jeu\npour connecter le jeu au serveur PP2.\nÀ utiliser si la modification du fichier host ne fonctionne pas", //15
            "Ouvre le navigateur de mods pour gérer les mods dans cette instance du jeu",                                                 //16
            "Enregistrer le profil",                                                                                                       //17
            "Se connecter au cloud",                                                                                                       //18
            "Se déconnecter du cloud",                                                                                                     //19
            "Actualiser les données du serveur",                                                                                           //20
            "Créer une sauvegarde locale\nOff | On",                                                                                       //21
            "Ouvrir le dossier des sauvegardes",                                                                                           //22
            "Créer une sauvegarde du jeu avant ou après le lancement/arrêt",                                                              //23
            "Nombre maximum de sauvegardes à conserver",                                                                                  //24
            "Ouvrir le dossier des sauvegardes",                                                                                           //25
            "Télécharger une sauvegarde depuis le cloud",                                                                                 //26
            "Téléverser une sauvegarde vers le cloud",
        };

        public static string[] IndonesianToolTip =
        {
            "Integrasi game dengan Discord\nOff | On – Menampilkan atau menyembunyikan status",                                               //0
            "Pemeriksaan pembaruan launcher\nOff | On – Memeriksa versi launcher terbaru",                                                    //1
            "Universal Plug and Play\nOff | On – Mencoba mengatur port forwarding secara otomatis",                                           //2
            "Bahasa yang digunakan launcher",                                                                                                //3
            "Mode audio: Standar atau Lanjutan\nTDU2 memiliki dua mode audio, Anda dapat beralih di antara keduanya",                          //4
            "Buat profil game baru",                                                                                                          //5
            "RAM dinamis\nOff | On – Memungkinkan aplikasi menggunakan lebih banyak RAM",                                                     //6
            "Core dinamis\nOff | On – Mencoba menetapkan lebih banyak inti CPU",                                                              //7
            "Prioritas dinamis\nOff | On – Menetapkan proses ke prioritas yang lebih tinggi dari normal",                                      //8
            "Kotoran kendaraan\nOff | On – Kendaraan dapat menjadi kotor",                                                                    //9
            "Kerusakan kendaraan\nOff | On – Kendaraan dapat mengalami kerusakan",                                                            //10
            "Mode online\nOff | On – Memulai game dalam mode online atau offline",                                                            //11
            "Pilih game yang akan digunakan",                                                                                                 //12
            "Buka folder game",                                                                                                                //13
            "Pembaruan game\nMemeriksa file yang hilang atau rusak",                                                                          //14
            "Mengubah konfigurasi game\nuntuk menghubungkan game ke server PP2.\nHarus digunakan jika pengeditan file host tidak berhasil",    //15
            "Membuka browser mod untuk mengelola mod di instance game ini",                                                                    //16
            "Simpan profil",                                                                                                                   //17
            "Masuk ke cloud",                                                                                                                  //18
            "Keluar dari cloud",                                                                                                               //19
            "Perbarui data server",                                                                                                            //20
            "Buat cadangan lokal\nOff | On",                                                                                                  //21
            "Buka folder cadangan",                                                                                                            //22
            "Buat cadangan savegame sebelum atau sesudah memulai/menutup game",                                                               //23
            "Jumlah maksimum savegame yang disimpan",                                                                                         //24
            "Buka folder savegame",                                                                                                            //25
            "Unduh savegame dari cloud",                                                                                                      //26
            "Unggah savegame ke cloud",
            ""
        };

        public static string[] ItalianToolTip =
        {
            "Integrazione del gioco con Discord\nOff | On – Mostra o nasconde lo stato",                                                     //0
            "Controllo aggiornamenti del launcher\nOff | On – Verifica nuove versioni del launcher",                                          //1
            "Universal Plug and Play\nOff | On – Tenta di configurare automaticamente il port forwarding",                                   //2
            "Lingua che il launcher deve usare",                                                                                             //3
            "Modalità audio: Standard o Avanzata\nTDU2 ha due modalità audio, puoi passare da una all’altra",                                 //4
            "Crea un nuovo profilo di gioco",                                                                                                //5
            "RAM dinamica\nOff | On – Permette all’applicazione di usare più RAM",                                                           //6
            "Core dinamici\nOff | On – Tenta di assegnare più core CPU",                                                                     //7
            "Priorità dinamica\nOff | On – Imposta il processo a una priorità superiore alla normale",                                       //8
            "Sporcizia del veicolo\nOff | On – I veicoli possono sporcarsi",                                                                //9
            "Danni ai veicoli\nOff | On – I veicoli possono subire danni",                                                                  //10
            "Modalità online\nOff | On – Avvia il gioco in modalità online o offline",                                                      //11
            "Seleziona il gioco da utilizzare",                                                                                             //12
            "Apri cartella del gioco",                                                                                                       //13
            "Aggiornamento del gioco\nVerifica file mancanti o corrotti",                                                                   //14
            "Modifica la configurazione del gioco\nper collegare il gioco al server PP2.\nDa usare se la modifica del file host non funziona", //15
            "Apri il browser delle mod per gestire le mod in questa istanza del gioco",                                                     //16
            "Salva profilo",                                                                                                                 //17
            "Accedi al cloud",                                                                                                               //18
            "Disconnetti dal cloud",                                                                                                         //19
            "Aggiorna dati del server",                                                                                                      //20
            "Crea backup locale\nOff | On",                                                                                                  //21
            "Apri cartella backup",                                                                                                          //22
            "Crea backup della partita prima o dopo l’avvio/arresto del gioco",                                                              //23
            "Numero massimo di salvataggi da conservare",                                                                                    //24
            "Apri cartella salvataggi",                                                                                                      //25
            "Scarica salvataggio dal cloud",                                                                                                 //26
            "Carica salvataggio nel cloud",
        };

        public static string[] LithuanianToolTip =
        {
            "Discord žaidimo integracija\nOff | On – Rodo arba slepia būseną",                                                             //0
            "Paleidimo programos atnaujinimų patikrinimas\nOff | On – Tikrina naujas paleidimo programos versijas",                          //1
            "Universalus Plug and Play\nOff | On – Bando automatiškai nustatyti uostų persiuntimą",                                        //2
            "Kalba, kurią turi naudoti paleidimo programa",                                                                                //3
            "Garso režimas: Standartinis arba Išplėstinis\nTDU2 turi du garso režimus, galite juos keisti",                                  //4
            "Sukurti naują žaidimo profilį",                                                                                                //5
            "Dinaminė RAM\nOff | On – Leidžia programai naudoti daugiau RAM",                                                               //6
            "Dinaminiai branduoliai\nOff | On – Bando priskirti daugiau CPU branduolių",                                                    //7
            "Dinaminis prioritetas\nOff | On – Nustato procesą aukštesniu prioritetu nei įprasta",                                          //8
            "Transporto priemonės purvas\nOff | On – Transporto priemonės gali užsiteršti",                                                  //9
            "Transporto priemonės pažeidimai\nOff | On – Transporto priemonės gali būti pažeistos",                                         //10
            "Internetinis režimas\nOff | On – Pradeda žaidimą internetiniame arba neprisijungusiame režime",                                 //11
            "Pasirinkite žaidimą, kuris bus naudojamas",                                                                                   //12
            "Atidaryti žaidimo aplanką",                                                                                                    //13
            "Žaidimo atnaujinimas\nTikrina trūkstamus arba sugadintus failus",                                                             //14
            "Pakeičia žaidimo konfigūraciją\nkad žaidimas prisijungtų prie PP2 serverio.\nTurėtų būti naudojama, jei hostfailo redagavimas neveikia", //15
            "Atidaro modų naršyklę modams valdyti šioje žaidimo instancijoje",                                                              //16
            "Išsaugoti profilį",                                                                                                           //17
            "Prisijungti prie debesies",                                                                                                    //18
            "Atsijungti nuo debesies",                                                                                                      //19
            "Atnaujinti serverio duomenis",                                                                                                 //20
            "Sukurti vietinę atsarginę kopiją\nOff | On",                                                                                   //21
            "Atidaryti atsarginių kopijų aplanką",                                                                                          //22
            "Sukurti žaidimo atsarginę kopiją prieš arba po žaidimo paleidimo/pabaigos",                                                     //23
            "Maksimalus išsaugotų žaidimų skaičius",                                                                                        //24
            "Atidaryti išsaugotų žaidimų aplanką",                                                                                          //25
            "Atsisiųsti išsaugotą žaidimą iš debesies",                                                                                     //26
            "Įkelti išsaugotą žaidimą į debesį",
        };

        public static string[] PolishToolTip =
        {
            "Integracja gry z Discordem\nWyłączone | Włączone – Pokazuje lub ukrywa status",                                                //0
            "Sprawdzanie aktualizacji launchera\nWyłączone | Włączone – Sprawdza nowe wersje launchera",                                     //1
            "Universal Plug and Play\nWyłączone | Włączone – Próbuje automatycznie ustawić przekierowanie portów",                          //2
            "Język, którego ma używać launcher",                                                                                           //3
            "Tryb dźwięku: Standardowy lub Zaawansowany\nTDU2 ma dwa tryby dźwięku, możesz przełączać się między nimi",                       //4
            "Utwórz nowy profil gry",                                                                                                       //5
            "Dynamiczna pamięć RAM\nWyłączone | Włączone – Pozwala aplikacji używać więcej pamięci RAM",                                     //6
            "Dynamiczne rdzenie\nWyłączone | Włączone – Próbuje przydzielić więcej rdzeni CPU",                                              //7
            "Dynamiczny priorytet\nWyłączone | Włączone – Ustawia proces na wyższy priorytet niż normalny",                                 //8
            "Brud pojazdów\nWyłączone | Włączone – Pojazdy mogą się zabrudzić",                                                             //9
            "Uszkodzenia pojazdów\nWyłączone | Włączone – Pojazdy mogą ulec uszkodzeniu",                                                   //10
            "Tryb online\nWyłączone | Włączone – Uruchamia grę w trybie online lub offline",                                                //11
            "Wybierz grę, która ma być używana",                                                                                            //12
            "Otwórz folder gry",                                                                                                            //13
            "Aktualizacja gry\nSprawdza brakujące lub uszkodzone pliki",                                                                    //14
            "Zmienia konfigurację gry\naby połączyć grę z serwerem PP2.\nPowinno być używane, jeśli edycja pliku hosta nie działa",          //15
            "Otwiera przeglądarkę modów, aby zarządzać modami w tej instancji gry",                                                         //16
            "Zapisz profil",                                                                                                                //17
            "Zaloguj się do chmury",                                                                                                        //18
            "Wyloguj się z chmury",                                                                                                         //19
            "Odśwież dane serwera",                                                                                                         //20
            "Twórz lokalne kopie zapasowe\nWyłączone | Włączone",                                                                           //21
            "Otwórz folder kopii zapasowych",                                                                                                //22
            "Twórz kopię zapasową zapisanej gry przed lub po rozpoczęciu/zakończeniu gry",                                                  //23
            "Maksymalna liczba przechowywanych zapisów",                                                                                    //24
            "Otwórz folder zapisów",                                                                                                        //25
            "Pobierz zapis z chmury",                                                                                                       //26
            "Prześlij zapis do chmury",
        };

        public static string[] RussianToolTip =
        {
            "Интеграция игры с Discord\nВыкл | Вкл – Показывает или скрывает статус",                                                      //0
            "Проверка обновлений лаунчера\nВыкл | Вкл – Проверяет наличие новых версий лаунчера",                                            //1
            "Universal Plug and Play\nВыкл | Вкл – Пытается автоматически настроить переадресацию портов",                                   //2
            "Язык, который должен использовать лаунчер",                                                                                   //3
            "Режим звука: Стандартный или Продвинутый\nВ TDU2 есть два режима звука, между ними можно переключаться",                         //4
            "Создать новый игровой профиль",                                                                                               //5
            "Динамическая RAM\nВыкл | Вкл – Позволяет приложению использовать больше оперативной памяти",                                   //6
            "Динамические ядра\nВыкл | Вкл – Пытается назначить больше ядер CPU",                                                           //7
            "Динамический приоритет\nВыкл | Вкл – Устанавливает процесс с более высоким приоритетом, чем обычно",                           //8
            "Грязь на транспортных средствах\nВыкл | Вкл – Транспортные средства могут загрязняться",                                        //9
            "Повреждения транспортных средств\nВыкл | Вкл – Транспортные средства могут получать повреждения",                               //10
            "Онлайн-режим\nВыкл | Вкл – Запускает игру в онлайн или оффлайн режиме",                                                        //11
            "Выберите игру для использования",                                                                                             //12
            "Открыть папку игры",                                                                                                          //13
            "Обновление игры\nПроверяет отсутствующие или повреждённые файлы",                                                             //14
            "Изменяет конфигурацию игры\nдля подключения к серверу PP2.\nСледует использовать, если редактирование файла hosts не работает", //15
            "Открывает браузер модов для управления модами в этой копии игры",                                                              //16
            "Сохранить профиль",                                                                                                           //17
            "Войти в облако",                                                                                                              //18
            "Выйти из облака",                                                                                                             //19
            "Обновить данные сервера",                                                                                                     //20
            "Создать локальную резервную копию\nВыкл | Вкл",                                                                               //21
            "Открыть папку резервных копий",                                                                                               //22
            "Создать резервную копию сохранения до или после запуска/завершения игры",                                                     //23
            "Максимальное количество сохранений для хранения",                                                                             //24
            "Открыть папку сохранений",                                                                                                    //25
            "Скачать сохранение из облака",                                                                                                //26
            "Загрузить сохранение в облако",
        };

        public static string[] Portuguese_BrazilToolTip =
        {
            "Integração do jogo com Discord\nOff | On – Mostra ou oculta o status",                                                        //0
            "Verificação de atualização do launcher\nOff | On – Verifica novas versões do launcher",                                       //1
            "Universal Plug and Play\nOff | On – Tenta configurar encaminhamento de porta automaticamente",                                //2
            "Idioma que o launcher deve usar",                                                                                             //3
            "Modo de áudio: Padrão ou Avançado\nTDU2 possui dois modos de áudio, você pode alternar entre eles",                            //4
            "Criar um novo perfil de jogo",                                                                                                //5
            "RAM dinâmica\nOff | On – Permite que o aplicativo use mais memória RAM",                                                      //6
            "Núcleos dinâmicos\nOff | On – Tenta atribuir mais núcleos da CPU",                                                            //7
            "Prioridade dinâmica\nOff | On – Define o processo com prioridade maior que a normal",                                         //8
            "Sujeira do veículo\nOff | On – Os veículos podem ficar sujos",                                                                //9
            "Danos ao veículo\nOff | On – Os veículos podem sofrer danos",                                                                 //10
            "Modo online\nOff | On – Inicia o jogo em modo online ou offline",                                                            //11
            "Selecione o jogo a ser usado",                                                                                                //12
            "Abrir pasta do jogo",                                                                                                          //13
            "Atualização do jogo\nVerifica arquivos ausentes ou corrompidos",                                                              //14
            "Altera a configuração do jogo\npara conectar o jogo ao servidor PP2.\nDeve ser usado se a edição do arquivo host não funcionar", //15
            "Abre o navegador de mods para gerenciar mods nesta instância do jogo",                                                        //16
            "Salvar perfil",                                                                                                                //17
            "Entrar na nuvem",                                                                                                              //18
            "Sair da nuvem",                                                                                                                //19
            "Atualizar dados do servidor",                                                                                                  //20
            "Criar backup local\nOff | On",                                                                                                 //21
            "Abrir pasta de backup",                                                                                                        //22
            "Criar backup do savegame antes ou depois do início/término do jogo",                                                          //23
            "Número máximo de savegames a manter",                                                                                         //24
            "Abrir pasta de savegames",                                                                                                     //25
            "Baixar savegame da nuvem",                                                                                                     //26
            "Enviar savegame para a nuvem",
        };

        public static string[] English_UKToolTip =
        {
            "Discord game integration\nOff | On – Shows or hides the status",                                                                  //0
            "Launcher update check\nOff | On – Checks for new launcher versions",                                                              //1
            "Universal Plug and Play\nOff | On – Attempts to set up port forwarding automatically",                                           //2
            "Language the launcher should use",                                                                                                 //3
            "Audio mode: Standard or Advanced\nTDU2 has two audio modes, you can switch between them",                                         //4
            "Create a new game profile",                                                                                                        //5
            "Dynamic RAM\nOff | On – Allows the application to use more RAM",                                                                  //6
            "Dynamic cores\nOff | On – Attempts to assign more CPU cores",                                                                     //7
            "Dynamic priority\nOff | On – Sets the process to a higher priority than normal",                                                  //8
            "Vehicle dirt\nOff | On – Vehicles can get dirty",                                                                                 //9
            "Vehicle damage\nOff | On – Vehicles can take damage",                                                                             //10
            "Online mode\nOff | On – Starts the game in online or offline mode",                                                              //11
            "Select the game to be used",                                                                                                       //12
            "Open the game folder",                                                                                                             //13
            "Game update\nChecks for missing or corrupted files",                                                                              //14
            "Changes the game configuration\nto connect the game to the PP2 server.\nShould be used if Hostfile-Edit does not work",          //15
            "Opens the mod browser to manage mods in this game instance",                                                                      //16
            "Save profile",                                                                                                                     //17
            "Log in to cloud",                                                                                                                  //18
            "Log out of cloud",                                                                                                                 //19
            "Refresh server data",                                                                                                              //20
            "Create local backup\nOff | On",                                                                                                    //21
            "Open backup folder",                                                                                                               //22
            "Create savegame backup before or after game start/end",                                                                           //23
            "Maximum number of savegames to keep",                                                                                             //24
            "Open savegame folder",                                                                                                             //25
            "Download savegame from cloud",                                                                                                     //26
            "Upload savegame to cloud",                                                                                                         //27
        };

        public static string[] DutchToolTip =
        {
            "Discord-game-integratie\nUit | Aan – Toont of verbergt de status",                                                          //0
            "Launcher-updatecontrole\nUit | Aan – Controleert op nieuwe launcher-versies",                                                //1
            "Universele Plug and Play\nUit | Aan – Probeert automatisch poortforwarding in te stellen",                                    //2
            "Taal die de launcher moet gebruiken",                                                                                        //3
            "Audiomodus: Standaard of Geavanceerd\nTDU2 heeft twee audiomodi, je kunt wisselen tussen beide",                              //4
            "Maak een nieuw gameprofiel",                                                                                                 //5
            "Dynamisch RAM\nUit | Aan – Staat de applicatie toe meer RAM te gebruiken",                                                    //6
            "Dynamische cores\nUit | Aan – Probeert meer CPU-cores toe te wijzen",                                                          //7
            "Dynamische prioriteit\nUit | Aan – Zet het proces op een hogere prioriteit dan normaal",                                      //8
            "Voertuigvuil\nUit | Aan – Voertuigen kunnen vies worden",                                                                    //9
            "Voertuigschade\nUit | Aan – Voertuigen kunnen schade oplopen",                                                                //10
            "Online modus\nUit | Aan – Start het spel in online- of offline-modus",                                                       //11
            "Selecteer het spel dat gebruikt moet worden",                                                                                //12
            "Open de game-map",                                                                                                            //13
            "Game-update\nControleert ontbrekende of beschadigde bestanden",                                                              //14
            "Wijzigt de gameconfiguratie\nom het spel te verbinden met de PP2-server.\nMoet gebruikt worden als het bewerken van het hostbestand niet werkt", //15
            "Opent de modbrowser om mods in deze game-instantie te beheren",                                                             //16
            "Profiel opslaan",                                                                                                             //17
            "Inloggen bij cloud",                                                                                                          //18
            "Uitloggen bij cloud",                                                                                                         //19
            "Servergegevens vernieuwen",                                                                                                   //20
            "Lokale back-up maken\nUit | Aan",                                                                                            //21
            "Open back-upmap",                                                                                                             //22
            "Back-up van savegame maken voor of na het starten/beëindigen van het spel",                                                   //23
            "Maximaal aantal savegames om te bewaren",                                                                                    //24
            "Open savegame-map",                                                                                                           //25
            "Savegame downloaden vanuit de cloud",                                                                                        //26
            "Savegame uploaden naar de cloud",
        };

        public static string[] RomanianToolTip =
        {
            "Integrare joc cu Discord\nOff | On – Afișează sau ascunde statusul",                                                        //0
            "Verificare actualizare launcher\nOff | On – Verifică versiuni noi ale launcher-ului",                                       //1
            "Universal Plug and Play\nOff | On – Încearcă să configureze automat redirecționarea porturilor",                            //2
            "Limba pe care trebuie să o folosească launcher-ul",                                                                         //3
            "Mod audio: Standard sau Avansat\nTDU2 are două moduri audio, poți comuta între ele",                                        //4
            "Creează un profil nou de joc",                                                                                              //5
            "RAM dinamică\nOff | On – Permite aplicației să folosească mai multă memorie RAM",                                          //6
            "Nuclee dinamice\nOff | On – Încearcă să aloce mai multe nuclee CPU",                                                        //7
            "Prioritate dinamică\nOff | On – Setează procesul cu o prioritate mai mare decât normal",                                   //8
            "Murdărie vehicul\nOff | On – Vehiculele se pot murdări",                                                                    //9
            "Deteriorare vehicul\nOff | On – Vehiculele pot suferi daune",                                                               //10
            "Mod online\nOff | On – Pornește jocul în modul online sau offline",                                                         //11
            "Selectează jocul care va fi folosit",                                                                                       //12
            "Deschide folderul jocului",                                                                                                 //13
            "Actualizare joc\nVerifică fișiere lipsă sau corupte",                                                                       //14
            "Modifică configurația jocului\npentru a conecta jocul la serverul PP2.\nTrebuie folosit dacă editarea fișierului host nu funcționează", //15
            "Deschide browser-ul de moduri pentru a gestiona moduri în această instanță a jocului",                                      //16
            "Salvează profilul",                                                                                                         //17
            "Conectează-te la cloud",                                                                                                    //18
            "Deconectează-te de la cloud",                                                                                                //19
            "Reîmprospătează datele serverului",                                                                                         //20
            "Creează backup local\nOff | On",                                                                                            //21
            "Deschide folderul backup-ului",                                                                                              //22
            "Creează backup înainte sau după pornirea/opritul jocului",                                                                  //23
            "Numărul maxim de salvări păstrate",                                                                                         //24
            "Deschide folderul cu salvări",                                                                                              //25
            "Descarcă salvare din cloud",                                                                                                //26
            "Încarcă salvare în cloud",
        };

        public static string[] TurkishToolTip =
        {
            "Discord oyun entegrasyonu\nKapalı | Açık – Durumu gösterir veya gizler",                                                      //0
            "Başlatıcı güncelleme kontrolü\nKapalı | Açık – Yeni başlatıcı sürümlerini kontrol eder",                                      //1
            "Universal Plug and Play\nKapalı | Açık – Port yönlendirmeyi otomatik olarak ayarlamayı dener",                                 //2
            "Başlatıcının kullanacağı dil",                                                                                                //3
            "Ses modu: Standart veya Gelişmiş\nTDU2’nin iki ses modu vardır, aralarında geçiş yapabilirsiniz",                               //4
            "Yeni oyun profili oluştur",                                                                                                   //5
            "Dinamik RAM\nKapalı | Açık – Uygulamanın daha fazla RAM kullanmasına izin verir",                                             //6
            "Dinamik çekirdekler\nKapalı | Açık – Daha fazla CPU çekirdeği atamayı dener",                                                 //7
            "Dinamik öncelik\nKapalı | Açık – Süreci normalden daha yüksek öncelikte çalıştırır",                                          //8
            "Araç kirlenmesi\nKapalı | Açık – Araçlar kirlenebilir",                                                                        //9
            "Araç hasarı\nKapalı | Açık – Araçlar hasar alabilir",                                                                         //10
            "Çevrimiçi mod\nKapalı | Açık – Oyunu çevrimiçi veya çevrimdışı modda başlatır",                                               //11
            "Kullanılacak oyunu seçin",                                                                                                    //12
            "Oyun klasörünü aç",                                                                                                           //13
            "Oyun güncellemesi\nEksik veya bozuk dosyaları kontrol eder",                                                                 //14
            "Oyun yapılandırmasını değiştirir\nPP2 sunucusuna bağlanmak için.\nHost dosyası düzenleme çalışmazsa kullanılmalıdır",         //15
            "Bu oyun örneğinde modları yönetmek için mod tarayıcısını açar",                                                               //16
            "Profili kaydet",                                                                                                              //17
            "Buluta giriş yap",                                                                                                            //18
            "Buluttan çıkış yap",                                                                                                          //19
            "Sunucu verilerini yenile",                                                                                                    //20
            "Yerel yedek oluştur\nKapalı | Açık",                                                                                           //21
            "Yedekleme klasörünü aç",                                                                                                      //22
            "Oyunun başlatılmasından/bitirilmesinden önce veya sonra savegame yedeği oluştur",                                             //23
            "Saklanacak maksimum savegame sayısı",                                                                                         //24
            "Savegame klasörünü aç",                                                                                                       //25
            "Buluttan savegame indir",                                                                                                     //26
            "Savegame’i buluta yükle",
        };

        public static string[] ChineseToolTip =
        {
            "Discord 游戏集成\n关闭 | 开启 – 显示或隐藏状态",                                                                             //0
            "启动器更新检查\n关闭 | 开启 – 检查启动器的新版本",                                                                           //1
            "通用即插即用\n关闭 | 开启 – 尝试自动设置端口转发",                                                                             //2
            "启动器使用的语言",                                                                                                             //3
            "音频模式：标准或高级\nTDU2 有两种音频模式，可在它们之间切换",                                                                   //4
            "创建新的游戏配置文件",                                                                                                         //5
            "动态内存\n关闭 | 开启 – 允许应用程序使用更多内存",                                                                             //6
            "动态核心\n关闭 | 开启 – 尝试分配更多 CPU 核心",                                                                               //7
            "动态优先级\n关闭 | 开启 – 将进程设置为高于正常的优先级",                                                                       //8
            "车辆污垢\n关闭 | 开启 – 车辆会变脏",                                                                                           //9
            "车辆损坏\n关闭 | 开启 – 车辆可能受损",                                                                                         //10
            "在线模式\n关闭 | 开启 – 以在线或离线模式启动游戏",                                                                            //11
            "选择要使用的游戏",                                                                                                             //12
            "打开游戏文件夹",                                                                                                               //13
            "游戏更新\n检查缺失或损坏的文件",                                                                                              //14
            "更改游戏配置\n以连接游戏到 PP2 服务器。\n如果 Hosts 文件编辑无效，应使用此选项",                                              //15
            "打开 Mod 浏览器以管理该游戏实例的 Mod",                                                                                        //16
            "保存配置文件",                                                                                                                 //17
            "登录云端",                                                                                                                     //18
            "退出云端",                                                                                                                     //19
            "刷新服务器数据",                                                                                                               //20
            "创建本地备份\n关闭 | 开启",                                                                                                     //21
            "打开备份文件夹",                                                                                                               //22
            "在游戏开始或结束前后创建存档备份",                                                                                             //23
            "保留存档的最大数量",                                                                                                           //24
            "打开存档文件夹",                                                                                                               //25
            "从云端下载存档",                                                                                                               //26
            "上传存档到云端",
        };

        public static string[] German =
        {
            //Main UI-Langs
            "Spielen",                                              //0
            "Start",                                                //1
            "Einstellungen",                                        //2
            "Profile",                                              //3
            "Service",                                              //4
            "Informationen",                                        //5
            "Über",                                                 //6
            "Beenden",                                              //7
            "Status prüfen",                                        //8
            "Zurück",                                               //9
            "Ersteller",                                            //10
            "Beschreibung",                                         //11
            "Updates",                                              //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Discord-Status",                                       //13
            "Update-Prüfung",                                       //14
            "UPnP-Portweiterleitung",                               //15
            "Sprache",                                              //16
            "Tonmodus",                                             //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Neues Profil anlegen",                                 //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Neue Spiel-Instanz erstellen",                         //19
            "Spiel-Instanz bearbeiten",                             //20
            "Profilname",                                           //21
            "Startargs",                                            //22
            "Dynamischer RAM",                                      //23
            "Dynamische Kerne",                                     //24
            "Höhere Priorität",                                     //25
            "Auto-Dreck",                                           //26
            "Auto-Schaden",                                         //27
            "Online-Modus",                                         //28
            "Spiel auswählen",                                      //29
            "Ordner öffnen",                                        //30
            "Verbindungs-Patch",                                    //31
            "Mod-Browser",                                          //32
            "Spiel-Update",                                         //33
            "Speichern",                                            //34
            "Spielversion: ",                                       //35
            "Spielbuild: {0}",                                      //36
            "Spielpfad: {0}",                                       //37
            "Spielinstallationsgröße: {0}",                         //38
            "Spieltyp: {0} '{1}' - {2}",                            //39
            " (Falsche Spielversion) Update erforderlich",          //40
            "Wähle dein Test Drive Unlimited Spiel",                //41
            "Config Datei wurde ausgetauscht",                      //42
            "Spiel Verzeichniss exestiert nicht.",                  //43
            "Ordner Exestiert Nicht: ",                             //44
            "Profil: {0} erfolgreich Gespeichert",                  //45
            "Fehler beim Speichern de Profils: {0} Fehler: {1}",    //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Account oder Password, kann nicht leer sein",          //47
            "Account oder Password, kann nicht leer sein",          //48
            "Password, kann nicht leer sein",                       //49
            "Falsche Login daten, Einloggen nicht Möglich",         //50
            "Fehler beim Login in der Cloud: {0}",                  //51
            "Status: {0}\nSpeichergröße: {1}\nZuletzt hochgeladen: {2}\nSavegame-Name: {3}\nLetzte Prüfung: {4}", //52
            "Savegame Exestiert",                                   //53
            "Kein Savegame Exestiert",                              //54
            "Fehler beim Laden der Savegame Infos: {0}",            //55
            "Savegame Hochladen",                                   //56
            "Upload-Fortschritt: {0}%, Größe: {1}",                 //57  
            "Download-Fortschritt: {0}%, Größe: {1}",               //58
            "Savegame Hochgeladen",                                 //59
            "Savegame Downloaden",                                  //60
            "Savegame Runtergeladen",                               //61
            "Savegame Entpacken",                                   //62
            "Temp-Files Cleanup",                                   //63
            "PP2 Account Info",                                     //64
            "Account name",                                         //65
            "Account password",                                     //66
            "Login",                                                //67
            "Logout",                                               //68
            "Refresh",                                              //69
            "Hochladen",                                            //70
            "Downloaden",                                           //71
            "Backup Ordner",                                        //72
            "Savegame Ordner",                                      //73
            "Max. Speicherpunkte",                                  //74
            "Cleanup",                                              //75
            //Service UI-Langs end
            //Update UI-Langs
            "Starte die Spiel überprüfung",                         //76
            "Unpacked Spiel erkannt, Stoppe Update",                //77
            "Keine normale Spiel Installation erkannt, Stoppe Update",//78
            "Gefunden Dateien: {0} Ordner: {1}",                    //79
            "Verbinde zum Server",                                  //80
            "Verbindung zum Server: OK, Starte Filecheck",          //81
            "Konnte nicht zum Server Verbinden",                    //82
            "Vergleiche Dateien, Phase 1",                          //83
            "Phase 1: Überprüfung auf fehlende Dateien",            //84
            "Fehlende Dateien: {0}",                                //85
            "Fehlt: {0} - {1}",                                     //86
            "Überprüfe Dateien auf Änderungen",                     //87
            "Vergleiche Dateien, Phase 2",                          //88
            "Phase 2: Datei wird überprüft: ",                      //89
            "Datei: {0} ◄ Aktualisierung erforderlich, Datei unterschiedlich [{1}|{2}]",//90
            "Datei: {0} ◄ Datei ist auf dem neuesten Stand",        //91
            "Die Dateiüberprüfung scheint fehlgeschlagen zu sein. Bitte versuchen Sie es erneut.", //92
            "Dateiüberprüfung abgeschlossen in: {0}",               //93
            "Online-Spieldateiliste konnte nicht abgerufen werden. Bitte versuchen Sie es später erneut.",//94
            "Live-Dateiliste heruntergeladen.",                     //95
            "Zu viele fehlende Dateien. Aktualisierung abgebrochen. Bitte installiere das Spiel neu.", //96
            "Dateien und Ordner gescannt.",                         //97
            "Fehlendes Verzeichnis erstellt: ",                     //98
            "Vergleiche Dateien, Phase 3",                          //99
            "Phase 3: Überprüfung der Dateiintegrität",             //100
            "Phase 3: Herunterladen und Installieren von Dateien",  //101
            "Herunterladen der Datei: {0} ({1}% abgeschlossen)",    //102
            "Aktualisierung abgeschlossen in: {0}",                 //103
            "Spielaktualisierung beendet.",                         //104
            "Datei aktualisiert: ",                                 //105
            "Entpacken der Datei fehlgeschlagen: ({0}). Fehler: {1}", //106
            "Datei: {0} ◄ Wird installiert",                        //107
            "Spiel Prüfung Fertig",                                 //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Security Module",                 //109
            "Das Sicherheitsmodul ist nicht sicher. Das Spiel wurde geschlossen.", //110
            "Es wurde ein Steam Spiel erkannt, ist das Richtig?", //111
            "Es wurde ein Unpacked Spiel erkannt, ist das Richtig?", //112
            "Warnung: Entpackte Spiele zu nutzen auf eigenes Risiko!\nWir geben hier keine Hilfe, oder sämmtlichen Support!", //113
            "Warnung: Sollte dies dennoch ein Unpacked Spiel sein, Kann es Passieren das Manche Aktionen das Spiel Kaputt machen kann!\nUnd zu einer Neuinstallation Führen kann", //114
            "Spiele-Server Online, Spieler: ",                  //115
            "Fehler beim Abrufen der Informationen, Klicken Sie hier, um zu aktualisieren", //116
            //Update UI-Langs end
            //Mod Detail View
            "Zurück", //117
            "Ersteller", //118
            "Beschreibung", //119
            "Version", //120
            "Dateien", //121
            "Download Größe", //122
            "Installation Größe", //123
            "Updates", //124
            "Installationen", //125
            "Install | Update", //126
            "Modname", //127
            "Starte Mod-Download", //128
            "Mod Installation ({0}) Abgeschlossen", //129
            "Installiere Mod ({0}) Datei: {1} von {2}", //130
            "Installierte Mods", //131
            "Zeige: {0} Mods von {1}", //132
            "Keine Mods Verfügbar für diesen Spiel Type", //133
            "Lade Mods", //134
            "Mod: {0} wurde erfolgreich Deinstalliert", //135
            "Unpacked Mods können nicht vom Launcher Deinstalliert werden.", //136
            "Falsche ModId Versuche es nach einem Restart", //137
            "Ein ganz besonderer Dank \nAn unser großartiges Team und unsere Patreons – eure Unterstützung, euer Engagement und eure Beiträge bedeuten uns sehr viel. \nVon der Hilfe bei Discord bis hin zum Betrieb der Server – ihr macht dieses Projekt möglich. Wir schätzen jeden Einzelnen von euch sehr.", //138
            "Der Launcher hat eine Alte Datenbank version, \nSoll versucht werden die Alten Einstellungen zu einem Neuen Profil zu Convertieren?", //139
            "Du musst selbst ein Spiel Profil Anlegen,\nBevor du Spielen Kannst.", //140
            "Bitte Prüfe die Profil einstellungen\nDas Profil wurde Erfolgreich angelegt als: Converted Profile", //141
        };

        public static string[] English =
        {
            //Main UI-Langs
            "Play",                                                 //0
            "Start",                                                //1
            "Settings",                                             //2
            "Profile",                                              //3
            "Service",                                              //4
            "Information",                                          //5
            "About",                                                //6
            "Exit",                                                 //7
            "Check Status",                                         //8
            "Back",                                                 //9
            "Creator",                                              //10
            "Description",                                          //11
            "Updates",                                              //12
            //Main UI-Langs end
        
            //Settings UI-Langs
            "Discord Status",                                       //13
            "Update Check",                                         //14
            "UPnP Port Forwarding",                                 //15
            "Language",                                             //16
            "Audio Mode",                                           //17
            //Settings UI-Langs end
        
            //Profile UI-Langs
            "Create New Profile",                                   //18
            //Profile UI-Langs end
        
            //Create Profile UI-Langs
            "Create New Game Instance",                             //19
            "Edit Game Instance",                                   //20
            "Profile Name",                                         //21
            "Start Args",                                      //22
            "Dynamic RAM",                                          //23
            "Dynamic Cores",                                        //24
            "Higher Priority",                                      //25
            "Auto-Dirt",                                            //26
            "Auto-Damage",                                          //27
            "Online Mode",                                          //28
            "Select Game",                                          //29
            "Open Folder",                                          //30
            "Connection Patch",                                     //31
            "Mod Browser",                                          //32
            "Game Update",                                          //33
            "Save",                                                 //34
            "Game Version: ",                                       //35
            "Game Build: {0}",                                      //36
            "Game Path: {0}",                                       //37
            "Game Install Size: {0}",                               //38
            "Game Type: {0} '{1}' - {2}",                           //39
            " (Wrong Game Version) Update Required",                //40
            "Choose your Test Drive Unlimited Game",                //41
            "Config File Replaced",                                 //42
            "Game Directory Does Not Exist",                        //43
            "Directory Does Not Exist: ",                           //44
            "Profile: {0} Saved Successfully",                      //45
            "Error Saving Profile: {0} Error: {1}",                 //46
            //Create Profile UI-Langs end
        
            //Service UI-Langs 
            "Account or Password cannot be empty",                  //47
            "Account or Password cannot be empty",                  //48
            "Password cannot be empty",                             //49
            "Incorrect Login Data, Cannot Log In",                  //50
            "Error Logging into Cloud: {0}",                        //51
            "Status: {0}\nStorage Size: {1}\nLast Uploaded: {2}\nSavegame Name: {3}\nLast Check: {4}", //52
            "Savegame Exists",                                      //53
            "No Savegame Exists",                                   //54
            "Error Loading Savegame Info: {0}",                     //55
            "Upload Savegame",                                      //56
            "Upload Progress: {0}%, Size: {1}",                     //57  
            "Download Progress: {0}%, Size: {1}",                   //58
            "Savegame Uploaded",                                    //59
            "Download Savegame",                                    //60
            "Savegame Downloaded",                                  //61
            "Savegame Extracting",                                  //62
            "Temp Files Cleanup",                                   //63
            "PP2 Account Info",                                     //64
            "Account Name",                                         //65
            "Account Password",                                     //66
            "Login",                                                //67
            "Logout",                                               //68
            "Refresh",                                              //69
            "Upload",                                               //70
            "Download",                                             //71
            "Backup Folder",                                        //72
            "Savegame Folder",                                      //73
            "Max Storage Points",                                   //74
            "Cleanup",                                              //75
            //Service UI-Langs end
        
            //Update UI-Langs
            "Starting Game Verification",                           //76
            "Unpacked Game Detected, Stopping Update",              //77
            "No Normal Game Installation Detected, Stopping Update",//78
            "Found Files: {0} Folders: {1}",                        //79
            "Connecting to Server",                                 //80
            "Connected to Server: OK, Starting File Check",         //81
            "Could Not Connect to Server",                          //82
            "Comparing Files, Phase 1",                             //83
            "Phase 1: Checking for Missing Files",                  //84
            "Missing Files: {0}",                                   //85
            "Missing: {0} - {1}",                                   //86
            "Checking Files for Changes",                           //87
            "Comparing Files, Phase 2",                             //88
            "Phase 2: Checking File: ",                             //89
            "File: {0} ◄ Update Required, File Differs [{1}|{2}]", //90
            "File: {0} ◄ File is Up to Date",                       //91
            "The file check seems to have failed. Please try again.", //92
            "File Check Completed in: {0}",                         //93
            "Online Game File List Could Not Be Retrieved. Please Try Again Later.",//94
            "Live File List Downloaded.",                           //95
            "Too Many Missing Files. Update Aborted. Please Reinstall the Game.", //96
            "Files and Folders Scanned.",                           //97
            "Missing Directory Created: ",                          //98
            "Comparing Files, Phase 3",                             //99
            "Phase 3: Checking File Integrity",                     //100
            "Phase 3: Downloading and Installing Files",            //101
            "Downloading File: {0} ({1}% Complete)",                //102
            "Update Completed in: {0}",                             //103
            "Game Update Finished.",                                //104
            "File Updated: ",                                       //105
            "Failed to Extract File: ({0}). Error: {1}",            //106
            "File: {0} ◄ Installing",                               //107
            "Game Check Complete",                                  //108
        
            //Gameupdate end
        
            //Generic Messages
            "Project Paradise 2 - Security Module",                 //109
            "The security module is not secure. The game has been closed.", //110
            "Steam Game Detected, Is This Correct?",                //111
            "Unpacked Game Detected, Is This Correct?",             //112
            "Warning: Using Unpacked Games at Your Own Risk!\nWe Provide No Help or Support Here!", //113
            "Warning: If This Is Still an Unpacked Game, Some Actions May Break the Game!\nAnd Can Lead to Reinstallation", //114
            "Game Servers Online, Players: ",                       //115
            "Error Fetching Information, Click Here to Refresh",    //116
        
            //Update UI-Langs end
        
            //Mod Detail View
            "Back",                                                 //117
            "Creator",                                              //118
            "Description",                                          //119
            "Version",                                              //120
            "Files",                                                //121
            "Download Size",                                        //122
            "Installation Size",                                    //123
            "Updates",                                              //124
            "Installations",                                        //125
            "Install | Update",                                     //126
            "Mod Name",                                             //127
            "Starting Mod Download",                                //128
            "Mod Installation ({0}) Complete",                      //129
            "Installing Mod ({0}) File: {1} of {2}",                //130
            "Installed Mods",                                       //131
            "Showing: {0} Mods of {1}",                             //132
            "No Mods Available for This Game Type",                 //133
            "Loading Mods",                                         //134
            "Mod: {0} Successfully Uninstalled",                    //135
            "Unpacked Mods Cannot Be Uninstalled by the Launcher",  //136
            "Incorrect ModId, Try Again After Restart",             //137
            "A Special Thanks \nTo Our Great Team and Our Patrons – Your Support, Engagement, and Contributions Mean a Lot to Us. \nFrom Discord Help to Server Operations – You Make This Project Possible. We Value Each One of You Very Much.", //138
            "The Launcher Has an Old Database Version, \nShould It Try to Convert Old Settings to a New Profile?", //139
            "You Must Create a Game Profile Yourself,\nBefore You Can Play.",                                       //140
            "Please Check the Profile Settings\nThe Profile Was Successfully Created As: Converted Profile",         //141
        };

        public static string[] Czech =
        {
            //Hlavní UI jazyky
            "Hrát",                                                 //0
            "Spustit",                                              //1
            "Nastavení",                                            //2
            "Profily",                                              //3
            "Služba",                                               //4
            "Informace",                                            //5
            "O programu",                                           //6
            "Ukončit",                                              //7
            "Zkontrolovat stav",                                    //8
            "Zpět",                                                 //9
            "Tvůrce",                                               //10
            "Popis",                                                //11
            "Aktualizace",                                          //12
            //Hlavní UI jazyky konec
        
            //Nastavení UI jazyky
            "Discord stav",                                         //13
            "Kontrola aktualizací",                                 //14
            "Přesměrování portů UPnP",                              //15
            "Jazyk",                                                //16
            "Zvukový režim",                                        //17
            //Nastavení UI jazyky konec
        
            //Profily UI jazyky
            "Vytvořit nový profil",                                 //18
            //Profily UI jazyky konec
        
            //Vytvoření profilu UI jazyky
            "Vytvořit novou herní instanci",                         //19
            "Upravit herní instanci",                               //20
            "Název profilu",                                        //21
            "Spustit arg",                                    //22
            "Dynamická RAM",                                        //23
            "Dynamické jádra",                                      //24
            "Vyšší priorita",                                       //25
            "Auto-prach",                                           //26
            "Auto-poškození",                                       //27
            "Online režim",                                         //28
            "Vybrat hru",                                           //29
            "Otevřít složku",                                       //30
            "Připojení patch",                                      //31
            "Prohlížeč modifikací",                                 //32
            "Aktualizace hry",                                      //33
            "Uložit",                                               //34
            "Verze hry: ",                                          //35
            "Sestavení hry: {0}",                                   //36
            "Cesta hry: {0}",                                       //37
            "Velikost instalace hry: {0}",                          //38
            "Typ hry: {0} '{1}' - {2}",                             //39
            " (Špatná verze hry) Vyžadována aktualizace",           //40
            "Vyberte svou hru Test Drive Unlimited",                //41
            "Konfigurační soubor byl nahrazen",                     //42
            "Adresář hry neexistuje",                               //43
            "Adresář neexistuje: ",                                  //44
            "Profil: {0} úspěšně uložen",                           //45
            "Chyba při ukládání profilu: {0} Chyba: {1}",          //46
            //Vytvoření profilu UI jazyky konec
        
            //Služba UI jazyky 
            "Účet nebo heslo nemůže být prázdné",                   //47
            "Účet nebo heslo nemůže být prázdné",                   //48
            "Heslo nemůže být prázdné",                             //49
            "Nesprávné přihlašovací údaje, nelze se přihlásit",      //50
            "Chyba při přihlášení do cloudu: {0}",                  //51
            "Stav: {0}\nVelikost úložiště: {1}\nNaposledy nahráno: {2}\nNázev uložené hry: {3}\nPoslední kontrola: {4}", //52
            "Uložená hra existuje",                                  //53
            "Žádná uložená hra neexistuje",                          //54
            "Chyba při načítání informací o uložené hře: {0}",      //55
            "Nahrát uloženou hru",                                  //56
            "Postup nahrávání: {0}%, Velikost: {1}",                //57  
            "Postup stahování: {0}%, Velikost: {1}",                //58
            "Uložená hra nahrána",                                  //59
            "Stáhnout uloženou hru",                                //60
            "Uložená hra stažena",                                  //61
            "Rozbalování uložené hry",                              //62
            "Vyčištění dočasných souborů",                          //63
            "Informace o účtu PP2",                                 //64
            "Název účtu",                                           //65
            "Heslo účtu",                                           //66
            "Přihlásit se",                                         //67
            "Odhlásit se",                                          //68
            "Obnovit",                                              //69
            "Nahrát",                                               //70
            "Stáhnout",                                             //71
            "Záložní složka",                                       //72
            "Složka uložených her",                                 //73
            "Maximální počet úložišť",                              //74
            "Vyčistit",                                             //75
            //Služba UI jazyky konec
        
            //Aktualizace UI jazyky
            "Spouštění kontroly hry",                               //76
            "Zjištěna nebalená hra, zastavuji aktualizaci",         //77
            "Nenalezena normální instalace hry, zastavuji aktualizaci",//78
            "Nalezeno souborů: {0} složek: {1}",                    //79
            "Připojování k serveru",                                //80
            "Připojeno k serveru: OK, spouštění kontroly souborů",  //81
            "Nelze se připojit k serveru",                          //82
            "Porovnávání souborů, fáze 1",                         //83
            "Fáze 1: Kontrola chybějících souborů",                //84
            "Chybějící soubory: {0}",                               //85
            "Chybí: {0} - {1}",                                     //86
            "Kontrola souborů na změny",                            //87
            "Porovnávání souborů, fáze 2",                         //88
            "Fáze 2: Kontrola souboru: ",                           //89
            "Soubor: {0} ◄ Vyžadována aktualizace, soubor se liší [{1}|{2}]",//90
            "Soubor: {0} ◄ Soubor je aktuální",                    //91
            "Kontrola souborů se nezdařila. Zkuste to prosím znovu.",//92
            "Kontrola souborů dokončena za: {0}",                   //93
            "Online seznam herních souborů nelze načíst. Zkuste to později.",//94
            "Seznam souborů stažen",                                //95
            "Příliš mnoho chybějících souborů. Aktualizace přerušena. Prosím, přeinstalujte hru.", //96
            "Soubory a složky prozkoumány.",                        //97
            "Vytvořená chybějící složka: ",                         //98
            "Porovnávání souborů, fáze 3",                         //99
            "Fáze 3: Kontrola integrity souborů",                  //100
            "Fáze 3: Stahování a instalace souborů",                //101
            "Stahování souboru: {0} ({1}% dokončeno)",              //102
            "Aktualizace dokončena za: {0}",                        //103
            "Aktualizace hry dokončena.",                           //104
            "Soubor aktualizován: ",                                //105
            "Nepodařilo se rozbalit soubor: ({0}). Chyba: {1}",     //106
            "Soubor: {0} ◄ Instalace",                              //107
            "Kontrola hry dokončena",                               //108
        
            //Aktualizace hry konec
        
            //Obecné zprávy
            "Project Paradise 2 - Bezpečnostní modul",              //109
            "Bezpečnostní modul není bezpečný. Hra byla uzavřena.", //110
            "Zjištěna hra ze Steamu, je to správné?",               //111
            "Zjištěna nebalená hra, je to správné?",                //112
            "Varování: Používání nebalených her na vlastní riziko!\nZde neposkytujeme žádnou pomoc ani podporu!", //113
            "Varování: Pokud je to stále nebalená hra, některé akce mohou poškodit hru!\nA mohou vést k přeinstalaci", //114
            "Herní servery online, hráči: ",                        //115
            "Chyba při načítání informací, klikněte sem pro obnovení", //116
        
            //Aktualizace UI jazyky konec
        
            //Detail modifikace
            "Zpět",                                                 //117
            "Tvůrce",                                               //118
            "Popis",                                                //119
            "Verze",                                                //120
            "Soubory",                                              //121
            "Velikost stahování",                                   //122
            "Velikost instalace",                                   //123
            "Aktualizace",                                          //124
            "Instalace",                                            //125
            "Instalovat | Aktualizovat",                            //126
            "Název modifikace",                                     //127
            "Spouštění stahování modifikace",                        //128
            "Instalace modifikace ({0}) dokončena",                 //129
            "Instalace modifikace ({0}) soubor: {1} z {2}",         //130
            "Nainstalované modifikace",                             //131
            "Zobrazuji: {0} modifikací z {1}",                      //132
            "Žádné modifikace nejsou dostupné pro tento typ hry",   //133
            "Načítání modifikací",                                  //134
            "Modifikace: {0} byla úspěšně odinstalována",          //135
            "Nezabalené modifikace nelze odinstalovat pomocí spouštěče", //136
            "Nesprávné ID modifikace, zkuste to znovu po restartu", //137
            "Zvláštní poděkování \nNaše skvělý tým a naši patroni – vaše podpora, zapojení a příspěvky pro nás mnoho znamenají. \nOd pomoci na Discordu až po provoz serverů – děláte tento projekt možný. Vážíme si každého z vás velmi.", //138
            "Spouštěč má starší verzi databáze, \nMá se pokusit převést staré nastavení na nový profil?", //139
            "Musíte si vytvořit herní profil sami,\nnež budete moci hrát.",                                        //140
            "Zkontrolujte prosím nastavení profilu\nProfil byl úspěšně vytvořen jako: Převedený profil",             //141
        };

        public static string[] Spanish =
        {
            //Main UI-Langs
            "Jugar",                                                //0
            "Iniciar",                                              //1
            "Configuración",                                        //2
            "Perfiles",                                             //3
            "Servicio",                                             //4
            "Información",                                          //5
            "Acerca de",                                            //6
            "Salir",                                                //7
            "Verificar Estado",                                     //8
            "Atrás",                                                //9
            "Creador",                                              //10
            "Descripción",                                          //11
            "Actualizaciones",                                      //12
            //Main UI-Langs end
        
            //Settings UI-Langs
            "Estado de Discord",                                    //13
            "Verificación de Actualizaciones",                      //14
            "Reenvío de Puertos UPnP",                              //15
            "Idioma",                                               //16
            "Modo de Audio",                                        //17
            //Settings UI-Langs end
        
            //Profile UI-Langs
            "Crear Nuevo Perfil",                                   //18
            //Profile UI-Langs end
        
            //Create Profile UI-Langs
            "Crear Nueva Instancia del Juego",                      //19
            "Editar Instancia del Juego",                           //20
            "Nombre del Perfil",                                    //21
            "Argumentos",                                 //22
            "RAM Dinámica",                                         //23
            "Núcleos Dinámicos",                                    //24
            "Prioridad Alta",                                       //25
            "Dirt Automático",                                      //26
            "Daño Automático",                                      //27
            "Modo Online",                                          //28
            "Seleccionar Juego",                                    //29
            "Abrir Carpeta",                                        //30
            "Parche de Conexión",                                   //31
            "Navegador de Mods",                                    //32
            "Actualización del Juego",                              //33
            "Guardar",                                              //34
            "Versión del Juego: ",                                  //35
            "Compilación del Juego: {0}",                           //36
            "Ruta del Juego: {0}",                                  //37
            "Tamaño de Instalación del Juego: {0}",                 //38
            "Tipo de Juego: {0} '{1}' - {2}",                       //39
            " (Versión Incorrecta del Juego) Actualización Requerida", //40
            "Elige tu Juego Test Drive Unlimited",                  //41
            "Archivo de Configuración Sustituido",                  //42
            "Directorio del Juego No Existe",                       //43
            "Directorio No Existe: ",                               //44
            "Perfil: {0} Guardado Exitosamente",                    //45
            "Error al Guardar el Perfil: {0} Error: {1}",           //46
            //Create Profile UI-Langs end
        
            //Service UI-Langs 
            "La cuenta o contraseña no pueden estar vacías",        //47
            "La cuenta o contraseña no pueden estar vacías",        //48
            "La contraseña no puede estar vacía",                   //49
            "Datos de inicio de sesión incorrectos, no se puede iniciar sesión", //50
            "Error al iniciar sesión en la nube: {0}",              //51
            "Estado: {0}\nTamaño del Almacenamiento: {1}\nÚltima Carga: {2}\nNombre del Guardado: {3}\nÚltima Verificación: {4}", //52
            "Guardado Existe",                                      //53
            "No Existe Guardado",                                   //54
            "Error al Cargar la Información del Guardado: {0}",     //55
            "Cargar Guardado",                                      //56
            "Progreso de Carga: {0}%, Tamaño: {1}",                 //57  
            "Progreso de Descarga: {0}%, Tamaño: {1}",              //58
            "Guardado Cargado",                                     //59
            "Descargar Guardado",                                   //60
            "Guardado Descargado",                                  //61
            "Extrayendo Guardado",                                  //62
            "Limpieza de Archivos Temporales",                      //63
            "Información de Cuenta PP2",                            //64
            "Nombre de Cuenta",                                     //65
            "Contraseña de Cuenta",                                 //66
            "Iniciar Sesión",                                       //67
            "Cerrar Sesión",                                        //68
            "Actualizar",                                           //69
            "Cargar",                                               //70
            "Descargar",                                            //71
            "Carpeta de Respaldo",                                  //72
            "Carpeta de Guardados",                                 //73
            "Puntos de Almacenamiento Máximos",                    //74
            "Limpiar",                                              //75
            //Service UI-Langs end
        
            //Update UI-Langs
            "Iniciando Verificación del Juego",                     //76
            "Juego Desempaquetado Detectado, Deteniendo Actualización", //77
            "Ninguna Instalación Normal de Juego Detectada, Deteniendo Actualización", //78
            "Archivos Encontrados: {0} Carpetas: {1}",              //79
            "Conectando al Servidor",                               //80
            "Conectado al Servidor: OK, Iniciando Verificación de Archivos", //81
            "No Se Pudo Conectar al Servidor",                      //82
            "Comparando Archivos, Fase 1",                          //83
            "Fase 1: Verificando Archivos Faltantes",               //84
            "Archivos Faltantes: {0}",                              //85
            "Falta: {0} - {1}",                                     //86
            "Verificando Archivos en Busca de Cambios",             //87
            "Comparando Archivos, Fase 2",                          //88
            "Fase 2: Verificando Archivo: ",                        //89
            "Archivo: {0} ◄ Actualización Requerida, Archivo Diferente [{1}|{2}]", //90
            "Archivo: {0} ◄ Archivo Está Actualizado",              //91
            "La verificación de archivos parece haber fallado. Por favor inténtalo de nuevo.", //92
            "Verificación de Archivos Completada en: {0}",          //93
            "No Se Pudo Obtener la Lista de Archivos del Juego En Línea. Por Favor Inténtalo Más Tarde.", //94
            "Lista de Archivos en Vivo Descargada.",                //95
            "Demasiados Archivos Faltantes. Actualización Abortada. Por Favor Reinstala el Juego.", //96
            "Archivos y Carpetas Escaneados.",                      //97
            "Directorio Faltante Creado: ",                         //98
            "Comparando Archivos, Fase 3",                          //99
            "Fase 3: Verificando Integridad de Archivos",           //100
            "Fase 3: Descargando e Instalando Archivos",            //101
            "Descargando Archivo: {0} ({1}% Completado)",           //102
            "Actualización Completada en: {0}",                     //103
            "Actualización del Juego Terminada.",                   //104
            "Archivo Actualizado: ",                                //105
            "Error al Extraer Archivo: ({0}). Error: {1}",          //106
            "Archivo: {0} ◄ Instalando",                            //107
            "Verificación del Juego Completada",                    //108
        
            //Gameupdate end
        
            //Generic Messages
            "Módulo de Seguridad de Project Paradise 2",            //109
            "El módulo de seguridad no es seguro. El juego ha sido cerrado.", //110
            "Juego de Steam Detectado, ¿Es Correcto?",              //111
            "Juego Desempaquetado Detectado, ¿Es Correcto?",        //112
            "Advertencia: ¡Usar Juegos Desempaquetados bajo Tu Propio Riesgo!\n¡No Ofrecemos Ayuda o Soporte Aquí!", //113
            "Advertencia: Si Este Es Todavía un Juego Desempaquetado, Algunas Acciones Pueden Dañar el Juego!\nY Pueden Llevar a una Reinstalación", //114
            "Servidores del Juego En Línea, Jugadores: ",           //115
            "Error al Obtener Información, Haz Clic Aquí para Actualizar", //116
        
            //Update UI-Langs end
        
            //Mod Detail View
            "Atrás",                                                //117
            "Creador",                                              //118
            "Descripción",                                          //119
            "Versión",                                              //120
            "Archivos",                                             //121
            "Tamaño de Descarga",                                   //122
            "Tamaño de Instalación",                                //123
            "Actualizaciones",                                      //124
            "Instalaciones",                                        //125
            "Instalar | Actualizar",                                //126
            "Nombre del Mod",                                       //127
            "Iniciando Descarga de Mod",                            //128
            "Instalación del Mod ({0}) Completada",                 //129
            "Instalando Mod ({0}) Archivo: {1} de {2}",             //130
            "Mods Instalados",                                      //131
            "Mostrando: {0} Mods de {1}",                           //132
            "No Hay Mods Disponibles para Este Tipo de Juego",      //133
            "Cargando Mods",                                        //134
            "Mod: {0} Desinstalado Exitosamente",                   //135
            "Mods Desempaquetados No Pueden Ser Desinstalados por el Iniciador", //136
            "ID de Mod Incorrecto, Inténtalo Nuevamente Después de Reiniciar", //137
            "Un Agradecimiento Especial \nA Nuestro Excelente Equipo y Nuestros Patrocinadores – Su Apoyo, Compromiso y Contribuciones Significan Mucho Para Nosotros. \nDesde la Ayuda en Discord hasta el Funcionamiento de los Servidores – Ustedes Hacen Este Proyecto Posible. Valoramos Muy a Cada Uno de Ustedes.", //138
            "El Iniciador Tiene una Versión Antigua de la Base de Datos, \n¿Debería Intentar Convertir la Configuración Antigua a un Nuevo Perfil?", //139
            "Debes Crear Tu Propio Perfil de Juego,\nAntes de Poder Jugar.",                                       //140
            "Por Favor Revisa la Configuración del Perfil\nEl Perfil Fue Creado Exitosamente Como: Perfil Convertido", //141
        };

        public static string[] French =
        {
            //Main UI-Langs
            "Jouer",                                                //0
            "Démarrer",                                             //1
            "Paramètres",                                           //2
            "Profils",                                              //3
            "Service",                                              //4
            "Informations",                                         //5
            "À propos",                                             //6
            "Quitter",                                              //7
            "Vérifier l'état",                                      //8
            "Retour",                                               //9
            "Créateur",                                             //10
            "Description",                                          //11
            "Mises à jour",                                         //12
            //Main UI-Langs end
        
            //Settings UI-Langs
            "Statut Discord",                                       //13
            "Vérification des mises à jour",                        //14
            "Redirection de port UPnP",                             //15
            "Langue",                                               //16
            "Mode audio",                                           //17
            //Settings UI-Langs end
        
            //Profile UI-Langs
            "Créer un nouveau profil",                              //18
            //Profile UI-Langs end
        
            //Create Profile UI-Langs
            "Créer une nouvelle instance de jeu",                   //19
            "Modifier l'instance de jeu",                           //20
            "Nom du profil",                                        //21
            "Arguments",                               //22
            "RAM dynamique",                                        //23
            "Cœurs dynamiques",                                     //24
            "Priorité élevée",                                      //25
            "Dirt automatique",                                     //26
            "Dégât automatique",                                    //27
            "Mode en ligne",                                        //28
            "Sélectionner le jeu",                                  //29
            "Ouvrir le dossier",                                    //30
            "Patch de connexion",                                   //31
            "Navigateur de mods",                                   //32
            "Mise à jour du jeu",                                   //33
            "Enregistrer",                                          //34
            "Version du jeu : ",                                    //35
            "Build du jeu : {0}",                                   //36
            "Chemin du jeu : {0}",                                  //37
            "Taille d'installation du jeu : {0}",                   //38
            "Type de jeu : {0} '{1}' - {2}",                        //39
            " (Mauvaise version du jeu) Mise à jour requise",       //40
            "Choisissez votre jeu Test Drive Unlimited",            //41
            "Fichier de configuration remplacé",                    //42
            "Le répertoire du jeu n'existe pas",                    //43
            "Le répertoire n'existe pas : ",                        //44
            "Profil : {0} enregistré avec succès",                  //45
            "Erreur lors de l'enregistrement du profil : {0} Erreur : {1}", //46
            //Create Profile UI-Langs end
        
            //Service UI-Langs 
            "Le compte ou le mot de passe ne peut pas être vide",   //47
            "Le compte ou le mot de passe ne peut pas être vide",   //48
            "Le mot de passe ne peut pas être vide",                //49
            "Identifiants de connexion incorrects, impossible de se connecter", //50
            "Erreur lors de la connexion au cloud : {0}",           //51
            "État : {0}\nTaille de stockage : {1}\nDernier téléchargement : {2}\nNom du sauvegarde : {3}\nDernière vérification : {4}", //52
            "Sauvegarde existante",                                 //53
            "Aucune sauvegarde existante",                          //54
            "Erreur lors du chargement des informations de sauvegarde : {0}", //55
            "Télécharger la sauvegarde",                            //56
            "Progression du téléchargement : {0}%, Taille : {1}",    //57  
            "Progression du téléchargement : {0}%, Taille : {1}",    //58
            "Sauvegarde téléchargée",                               //59
            "Télécharger la sauvegarde",                            //60
            "Sauvegarde téléchargée",                               //61
            "Extraction de la sauvegarde",                          //62
            "Nettoyage des fichiers temporaires",                   //63
            "Informations du compte PP2",                           //64
            "Nom du compte",                                        //65
            "Mot de passe du compte",                               //66
            "Se connecter",                                         //67
            "Se déconnecter",                                       //68
            "Actualiser",                                           //69
            "Télécharger",                                          //70
            "Télécharger",                                          //71
            "Dossier de sauvegarde",                                //72
            "Dossier des sauvegardes",                              //73
            "Points de stockage maximaux",                          //74
            "Nettoyer",                                             //75
            //Service UI-Langs end
        
            //Update UI-Langs
            "Démarrage de la vérification du jeu",                  //76
            "Jeu non empaqueté détecté, arrêt de la mise à jour",   //77
            "Aucune installation normale de jeu détectée, arrêt de la mise à jour", //78
            "Fichiers trouvés : {0} Dossiers : {1}",                //79
            "Connexion au serveur",                                 //80
            "Connecté au serveur : OK, démarrage de la vérification des fichiers", //81
            "Impossible de se connecter au serveur",                //82
            "Comparaison des fichiers, Phase 1",                    //83
            "Phase 1 : Vérification des fichiers manquants",        //84
            "Fichiers manquants : {0}",                             //85
            "Manquant : {0} - {1}",                                 //86
            "Vérification des fichiers pour les modifications",     //87
            "Comparaison des fichiers, Phase 2",                    //88
            "Phase 2 : Vérification du fichier : ",                 //89
            "Fichier : {0} ◄ Mise à jour requise, fichier différent [{1}|{2}]", //90
            "Fichier : {0} ◄ Fichier à jour",                       //91
            "La vérification des fichiers semble avoir échoué. Veuillez réessayer.", //92
            "Vérification des fichiers terminée en : {0}",          //93
            "Impossible de récupérer la liste des fichiers du jeu en ligne. Veuillez réessayer plus tard.", //94
            "Liste des fichiers en direct téléchargée.",            //95
            "Trop de fichiers manquants. Mise à jour annulée. Veuillez réinstaller le jeu.", //96
            "Fichiers et dossiers scannés.",                        //97
            "Répertoire manquant créé : ",                          //98
            "Comparaison des fichiers, Phase 3",                    //99
            "Phase 3 : Vérification de l'intégrité des fichiers",   //100
            "Phase 3 : Téléchargement et installation des fichiers", //101
            "Téléchargement du fichier : {0} ({1}% terminé)",       //102
            "Mise à jour terminée en : {0}",                        //103
            "Mise à jour du jeu terminée.",                         //104
            "Fichier mis à jour : ",                                //105
            "Échec de l'extraction du fichier : ({0}). Erreur : {1}", //106
            "Fichier : {0} ◄ Installation",                         //107
            "Vérification du jeu terminée",                         //108
        
            //Gameupdate end
        
            //Generic Messages
            "Module de sécurité de Project Paradise 2",             //109
            "Le module de sécurité n'est pas sécurisé. Le jeu a été fermé.", //110
            "Jeu Steam détecté, est-ce correct ?",                  //111
            "Jeu non empaqueté détecté, est-ce correct ?",          //112
            "Avertissement : Utiliser des jeux non empaquetés à vos propres risques !\nNous ne fournissons aucune aide ni support ici !", //113
            "Avertissement : Si c'est toujours un jeu non empaqueté, certaines actions peuvent endommager le jeu !\nEt peuvent mener à une réinstallation", //114
            "Serveurs de jeu en ligne, joueurs : ",                 //115
            "Erreur lors de la récupération des informations, cliquez ici pour actualiser", //116
        
            //Update UI-Langs end
        
            //Mod Detail View
            "Retour",                                               //117
            "Créateur",                                             //118
            "Description",                                          //119
            "Version",                                              //120
            "Fichiers",                                             //121
            "Taille de téléchargement",                             //122
            "Taille d'installation",                                //123
            "Mises à jour",                                         //124
            "Installations",                                        //125
            "Installer | Mettre à jour",                            //126
            "Nom du mod",                                           //127
            "Démarrage du téléchargement du mod",                   //128
            "Installation du mod ({0}) terminée",                   //129
            "Installation du mod ({0}) Fichier : {1} sur {2}",      //130
            "Mods installés",                                       //131
            "Affichage : {0} mods sur {1}",                         //132
            "Aucun mod disponible pour ce type de jeu",             //133
            "Chargement des mods",                                  //134
            "Mod : {0} désinstallé avec succès",                    //135
            "Les mods non empaquetés ne peuvent pas être désinstallés par le lanceur", //136
            "ID de mod incorrect, essayez à nouveau après redémarrage", //137
            "Un merci spécial \nÀ notre merveilleux équipe et nos parrains – votre soutien, engagement et contributions comptent beaucoup pour nous. \nDe l'aide sur Discord jusqu'au fonctionnement des serveurs – vous rendez ce projet possible. Nous apprécions chacun d'entre vous très sincerement.", //138
            "Le lanceur a une ancienne version de la base de données, \nDevrait-il essayer de convertir les anciens paramètres vers un nouveau profil ?", //139
            "Vous devez créer votre propre profil de jeu,\nAvant de pouvoir jouer.",                                //140
            "Veuillez vérifier les paramètres du profil\nLe profil a été créé avec succès en tant que : Profil Converti", //141
        };

        public static string[] Indonesian =
        {
            //Main UI-Langs
            "Bermain",                                              //0
            "Mulai",                                                //1
            "Pengaturan",                                           //2
            "Profil",                                               //3
            "Layanan",                                              //4
            "Informasi",                                            //5
            "Tentang",                                              //6
            "Keluar",                                               //7
            "Periksa Status",                                       //8
            "Kembali",                                              //9
            "Pembuat",                                              //10
            "Deskripsi",                                            //11
            "Pembaruan",                                            //12
            //Main UI-Langs end
        
            //Settings UI-Langs
            "Status Discord",                                       //13
            "Pemeriksaan Pembaruan",                               //14
            "Penerusan Port UPnP",                                  //15
            "Bahasa",                                               //16
            "Mode Audio",                                           //17
            //Settings UI-Langs end
        
            //Profile UI-Langs
            "Buat Profil Baru",                                     //18
            //Profile UI-Langs end
        
            //Create Profile UI-Langs
            "Buat Instance Permainan Baru",                         //19
            "Edit Instance Permainan",                              //20
            "Nama Profil",                                          //21
            "Argumen",                                        //22
            "RAM Dinamis",                                          //23
            "Inti Dinamis",                                         //24
            "Prioritas Tinggi",                                     //25
            "Dirt Otomatis",                                        //26
            "Kerusakan Otomatis",                                   //27
            "Mode Online",                                          //28
            "Pilih Permainan",                                      //29
            "Buka Folder",                                          //30
            "Patch Koneksi",                                        //31
            "Peramban Mod",                                         //32
            "Pembaruan Permainan",                                  //33
            "Simpan",                                               //34
            "Versi Permainan: ",                                    //35
            "Build Permainan: {0}",                                 //36
            "Jalur Permainan: {0}",                                 //37
            "Ukuran Instalasi Permainan: {0}",                      //38
            "Tipe Permainan: {0} '{1}' - {2}",                      //39
            " (Versi Permainan Salah) Pembaruan Diperlukan",        //40
            "Pilih permainan Test Drive Unlimited Anda",            //41
            "File Konfigurasi Diganti",                             //42
            "Direktori Permainan Tidak Ada",                        //43
            "Direktori Tidak Ada: ",                                //44
            "Profil: {0} Berhasil Disimpan",                        //45
            "Kesalahan saat menyimpan profil: {0} Kesalahan: {1}",  //46
            //Create Profile UI-Langs end
        
            //Service UI-Langs 
            "Akun atau kata sandi tidak boleh kosong",             //47
            "Akun atau kata sandi tidak boleh kosong",             //48
            "Kata sandi tidak boleh kosong",                        //49
            "Data masuk salah, tidak dapat masuk",                  //50
            "Kesalahan saat masuk ke cloud: {0}",                   //51
            "Status: {0}\nUkuran Penyimpanan: {1}\nTerakhir Diunggah: {2}\nNama Simpanan: {3}\nPeriksa Terakhir: {4}", //52
            "Simpanan Ada",                                         //53
            "Tidak Ada Simpanan",                                   //54
            "Kesalahan saat memuat informasi simpanan: {0}",        //55
            "Unggah Simpanan",                                      //56
            "Progres Unggah: {0}%, Ukuran: {1}",                    //57  
            "Progres Unduh: {0}%, Ukuran: {1}",                     //58
            "Simpanan Diunggah",                                    //59
            "Unduh Simpanan",                                       //60
            "Simpanan Diunduh",                                     //61
            "Mengekstrak Simpanan",                                 //62
            "Pembersihan File Sementara",                           //63
            "Informasi Akun PP2",                                   //64
            "Nama Akun",                                            //65
            "Kata Sandi Akun",                                      //66
            "Masuk",                                                //67
            "Keluar",                                               //68
            "Segarkan",                                             //69
            "Unggah",                                               //70
            "Unduh",                                                //71
            "Folder Cadangan",                                      //72
            "Folder Simpanan",                                      //73
            "Poin Penyimpanan Maksimum",                            //74
            "Bersihkan",                                            //75
            //Service UI-Langs end
        
            //Update UI-Langs
            "Memulai Verifikasi Permainan",                         //76
            "Permainan Tidak Terkemas Terdeteksi, Menghentikan Pembaruan", //77
            "Tidak Ada Instalasi Permainan Normal Terdeteksi, Menghentikan Pembaruan", //78
            "File Ditemukan: {0} Folder: {1}",                      //79
            "Menghubungkan ke Server",                              //80
            "Terhubung ke Server: OK, Memulai Pemeriksaan File",    //81
            "Tidak Dapat Terhubung ke Server",                      //82
            "Membandingkan File, Tahap 1",                          //83
            "Tahap 1: Memeriksa File yang Hilang",                  //84
            "File yang Hilang: {0}",                                //85
            "Hilang: {0} - {1}",                                    //86
            "Memeriksa File untuk Perubahan",                       //87
            "Membandingkan File, Tahap 2",                          //88
            "Tahap 2: Memeriksa File: ",                            //89
            "File: {0} ◄ Pembaruan Diperlukan, File Berbeda [{1}|{2}]", //90
            "File: {0} ◄ File Sudah Terbaru",                       //91
            "Pemeriksaan file tampaknya gagal. Silakan coba lagi.", //92
            "Pemeriksaan File Selesai dalam: {0}",                  //93
            "Daftar File Permainan Online Tidak Dapat Diambil. Silakan Coba Lagi Nanti.", //94
            "Daftar File Langsung Diunduh.",                        //95
            "Terlalu Banyak File yang Hilang. Pembaruan Dibatalkan. Silakan Instal Ulang Permainan.", //96
            "File dan Folder Dipindai.",                             //97
            "Direktori yang Hilang Dibuat: ",                       //98
            "Membandingkan File, Tahap 3",                          //99
            "Tahap 3: Memeriksa Integritas File",                   //100
            "Tahap 3: Mengunduh dan Menginstal File",              //101
            "Mengunduh File: {0} ({1}% Selesai)",                   //102
            "Pembaruan Selesai dalam: {0}",                         //103
            "Pembaruan Permainan Selesai.",                         //104
            "File Diperbarui: ",                                    //105
            "Gagal Mengekstrak File: ({0}). Kesalahan: {1}",        //106
            "File: {0} ◄ Menginstal",                               //107
            "Verifikasi Permainan Selesai",                         //108
        
            //Gameupdate end
        
            //Generic Messages
            "Modul Keamanan Project Paradise 2",                    //109
            "Modul keamanan tidak aman. Permainan telah ditutup.", //110
            "Permainan Steam Terdeteksi, Apakah Benar?",            //111
            "Permainan Tidak Terkemas Terdeteksi, Apakah Benar?",   //112
            "Peringatan: Menggunakan Permainan Tidak Terkemas dengan Risiko Anda Sendiri!\nKami Tidak Menyediakan Bantuan atau Dukungan Di Sini!", //113
            "Peringatan: Jika Ini Masih Permainan Tidak Terkemas, Beberapa Aksi Mungkin Merusak Permainan!\nDan Dapat Memerlukan Instalasi Ulang", //114
            "Server Permainan Online, Pemain: ",                    //115
            "Kesalahan Mengambil Informasi, Klik Di Sini untuk Segarkan", //116
        
            //Update UI-Langs end
        
            //Mod Detail View
            "Kembali",                                              //117
            "Pembuat",                                              //118
            "Deskripsi",                                            //119
            "Versi",                                                //120
            "File",                                                 //121
            "Ukuran Unduhan",                                       //122
            "Ukuran Instalasi",                                     //123
            "Pembaruan",                                            //124
            "Instalasi",                                            //125
            "Instal | Perbarui",                                    //126
            "Nama Mod",                                             //127
            "Memulai Unduhan Mod",                                  //128
            "Instalasi Mod ({0}) Selesai",                          //129
            "Menginstal Mod ({0}) File: {1} dari {2}",             //130
            "Mod Terinstal",                                        //131
            "Menampilkan: {0} Mod dari {1}",                        //132
            "Tidak Ada Mod Tersedia untuk Tipe Permainan Ini",      //133
            "Memuat Mod",                                           //134
            "Mod: {0} Berhasil Dihapus",                            //135
            "Mod Tidak Terkemas Tidak Bisa Dihapus oleh Launcher",  //136
            "ID Mod Salah, Coba Lagi Setelah Restart",              //137
            "Terima Kasih Khusus \nKepada Tim Hebat Kami dan Patreon kami – Dukungan, partisipasi, dan kontribusi Anda sangat berarti bagi kami. \nDari bantuan di Discord hingga operasional server – Anda membuat proyek ini mungkin. Kami menghargai setiap dari Anda sangat banyak.", //138
            "Launcher Memiliki Versi Database Lama, \nHaruskah Mencoba Mengonversi Pengaturan Lama ke Profil Baru?", //139
            "Anda Harus Membuat Profil Permainan Sendiri,\nSebelum Dapat Bermain.",                                //140
            "Silakan Periksa Pengaturan Profil\nProfil Berhasil Dibuat Sebagai: Profil Dikonversi",                //141
        };

        public static string[] Italian =
        {
            //Main UI-Langs
            "Gioca",                                                //0
            "Inizia",                                               //1
            "Impostazioni",                                         //2
            "Profili",                                              //3
            "Servizio",                                             //4
            "Informazioni",                                         //5
            "Informazioni su",                                      //6
            "Esci",                                                 //7
            "Controlla Stato",                                      //8
            "Indietro",                                             //9
            "Autore",                                               //10
            "Descrizione",                                          //11
            "Aggiornamenti",                                        //12
            //Main UI-Langs end
        
            //Settings UI-Langs
            "Stato Discord",                                        //13
            "Controllo Aggiornamenti",                              //14
            "Inoltro Porta UPnP",                                   //15
            "Lingua",                                               //16
            "Modalità Audio",                                       //17
            //Settings UI-Langs end
        
            //Profile UI-Langs
            "Crea Nuovo Profilo",                                   //18
            //Profile UI-Langs end
        
            //Create Profile UI-Langs
            "Crea Nuova Istanza Gioco",                             //19
            "Modifica Istanza Gioco",                               //20
            "Nome Profilo",                                         //21
            "Argomenti",                                      //22
            "RAM Dinamica",                                         //23
            "Core Dinamici",                                        //24
            "Priorità Alta",                                        //25
            "Dirt Automatico",                                      //26
            "Danno Automatico",                                     //27
            "Modalità Online",                                      //28
            "Seleziona Gioco",                                      //29
            "Apri Cartella",                                        //30
            "Patch Connessione",                                    //31
            "Browser Mod",                                          //32
            "Aggiornamento Gioco",                                  //33
            "Salva",                                                //34
            "Versione Gioco: ",                                     //35
            "Build Gioco: {0}",                                     //36
            "Percorso Gioco: {0}",                                  //37
            "Dimensione Installazione Gioco: {0}",                  //38
            "Tipo Gioco: {0} '{1}' - {2}",                          //39
            " (Versione Gioco Errata) Aggiornamento Richiesto",     //40
            "Scegli il tuo gioco Test Drive Unlimited",             //41
            "File Configurazione Sostituito",                       //42
            "Directory Gioco Non Esiste",                           //43
            "Directory Non Esiste: ",                               //44
            "Profilo: {0} Salvato Con Successo",                    //45
            "Errore durante il salvataggio del profilo: {0} Errore: {1}", //46
            //Create Profile UI-Langs end
        
            //Service UI-Langs 
            "Account o Password non possono essere vuoti",          //47
            "Account o Password non possono essere vuoti",          //48
            "Password non può essere vuota",                        //49
            "Credenziali di accesso errate, impossibile accedere",  //50
            "Errore durante l'accesso al cloud: {0}",               //51
            "Stato: {0}\nDimensione Archiviazione: {1}\nUltimo Caricamento: {2}\nNome Salvataggio: {3}\nUltimo Controllo: {4}", //52
            "Salvataggio Esiste",                                   //53
            "Nessun Salvataggio Esistente",                         //54
            "Errore durante il caricamento delle informazioni salvataggio: {0}", //55
            "Carica Salvataggio",                                   //56
            "Progresso Caricamento: {0}%, Dimensione: {1}",         //57  
            "Progresso Download: {0}%, Dimensione: {1}",            //58
            "Salvataggio Caricato",                                 //59
            "Scarica Salvataggio",                                  //60
            "Salvataggio Scaricato",                                //61
            "Estrazione Salvataggio",                               //62
            "Pulizia File Temporanei",                              //63
            "Informazioni Account PP2",                             //64
            "Nome Account",                                         //65
            "Password Account",                                     //66
            "Accedi",                                               //67
            "Esci",                                                 //68
            "Aggiorna",                                             //69
            "Carica",                                               //70
            "Scarica",                                              //71
            "Cartella Backup",                                      //72
            "Cartella Salvataggi",                                  //73
            "Punti Archiviazione Massimi",                          //74
            "Pulisci",                                              //75
            //Service UI-Langs end
        
            //Update UI-Langs
            "Avvio Verifica Gioco",                                 //76
            "Gioco Non Compresso Rilevato, Interrompo Aggiornamento", //77
            "Nessuna Installazione Normale Gioco Rilevata, Interrompo Aggiornamento", //78
            "File Trovati: {0} Cartelle: {1}",                      //79
            "Connessione al Server",                                //80
            "Connesso al Server: OK, Avvio Controllo File",         //81
            "Impossibile Connettersi al Server",                    //82
            "Confronto File, Fase 1",                               //83
            "Fase 1: Controllo File Mancanti",                      //84
            "File Mancanti: {0}",                                   //85
            "Mancante: {0} - {1}",                                  //86
            "Controllo File per Modifiche",                         //87
            "Confronto File, Fase 2",                               //88
            "Fase 2: Controllo File: ",                             //89
            "File: {0} ◄ Aggiornamento Richiesto, File Diverso [{1}|{2}]", //90
            "File: {0} ◄ File è Aggiornato",                        //91
            "Il controllo file sembra essere fallito. Per favore riprova.", //92
            "Controllo File Completato in: {0}",                    //93
            "Impossibile Recuperare Lista File Gioco Online. Riprova Più Tardi.", //94
            "Lista File Live Scaricata.",                           //95
            "Troppi File Mancanti. Aggiornamento Interrotto. Reinstalla il Gioco.", //96
            "File e Cartelle Scansionati.",                         //97
            "Cartella Mancante Creata: ",                           //98
            "Confronto File, Fase 3",                               //99
            "Fase 3: Controllo Integrità File",                     //100
            "Fase 3: Download e Installazione File",                //101
            "Download File: {0} ({1}% Completato)",                 //102
            "Aggiornamento Completato in: {0}",                     //103
            "Aggiornamento Gioco Terminato.",                       //104
            "File Aggiornato: ",                                    //105
            "Impossibile Estrarre File: ({0}). Errore: {1}",        //106
            "File: {0} ◄ Installazione",                            //107
            "Verifica Gioco Completata",                            //108
        
            //Gameupdate end
        
            //Generic Messages
            "Modulo Sicurezza Project Paradise 2",                  //109
            "Il modulo di sicurezza non è sicuro. Il gioco è stato chiuso.", //110
            "Gioco Steam Rilevato, È Corretto?",                    //111
            "Gioco Non Compresso Rilevato, È Corretto?",            //112
            "Attenzione: Usare Giochi Non Compressi a tuo rischio!\nNon forniamo aiuto o supporto qui!", //113
            "Attenzione: Se questo è ancora un gioco non compresso, alcune azioni potrebbero danneggiare il gioco!\nE possono portare a una reinstallazione", //114
            "Server Gioco Online, Giocatori: ",                     //115
            "Errore nel recupero informazioni, clicca qui per aggiornare", //116
        
            //Update UI-Langs end
        
            //Mod Detail View
            "Indietro",                                             //117
            "Autore",                                               //118
            "Descrizione",                                          //119
            "Versione",                                             //120
            "File",                                                 //121
            "Dimensione Download",                                  //122
            "Dimensione Installazione",                             //123
            "Aggiornamenti",                                        //124
            "Installazioni",                                        //125
            "Installa | Aggiorna",                                  //126
            "Nome Mod",                                             //127
            "Avvio Download Mod",                                   //128
            "Installazione Mod ({0}) Completata",                   //129
            "Installazione Mod ({0}) File: {1} di {2}",             //130
            "Mod Installate",                                       //131
            "Mostrando: {0} Mod di {1}",                            //132
            "Nessuna Mod Disponibile per Questo Tipo di Gioco",     //133
            "Caricamento Mod",                                      //134
            "Mod: {0} Disinstallata Con Successo",                  //135
            "Mod Non Compresso Non Possono Essere Disinstallate dal Launcher", //136
            "ID Mod Errato, Riprova Dopo Il Riavvio",               //137
            "Un Grazie Speciale \nAl Nostro Eccellente Team e Ai Nostri Patreon – Il Vostro Supporto, Impegno e Contributi Significano Molto Per Noi. \nDalla Guida su Discord fino al Funzionamento dei Server – Voi Rendete Questo Progetto Possibile. Vi Apprezziamo Ogni Singolo Di Voi Moltissimo.", //138
            "Il Launcher Ha Una Vecchia Versione Del Database, \nDovrebbe Provare A Convertire Le Vecchie Impostazioni In Un Nuovo Profilo?", //139
            "Devi Creare Il Tuo Profilo Di Gioco,\nPrima Di Poter Giocare.",                                       //140
            "Per Favore Controlla Le Impostazioni Del Profilo\nIl Profilo È Stato Creato Con Successo Come: Profilo Convertito", //141
        };

        public static string[] Lithuanian =
        {
            //Main UI-Langs
            "Žaisti",                                               //0
            "Paleisti",                                             //1
            "Nustatymai",                                           //2
            "Profiliai",                                            //3
            "Paslauga",                                             //4
            "Informacija",                                          //5
            "Apie",                                                 //6
            "Išeiti",                                               //7
            "Patikrinti būseną",                                    //8
            "Atgal",                                                //9
            "Kūrėjas",                                              //10
            "Aprašymas",                                            //11
            "Atnaujinimai",                                         //12
            //Main UI-Langs end
        
            //Settings UI-Langs
            "Discord statusas",                                     //13
            "Atnaujinimų tikrinimas",                               //14
            "UPnP prievadų nukreipimas",                            //15
            "Kalba",                                                //16
            "Garso režimas",                                        //17
            //Settings UI-Langs end
        
            //Profile UI-Langs
            "Sukurti naują profilį",                                //18
            //Profile UI-Langs end
        
            //Create Profile UI-Langs
            "Sukurti naują žaidimo instanciją",                     //19
            "Redaguoti žaidimo instanciją",                         //20
            "Profilio pavadinimas",                                 //21
            "argumentai",                                 //22
            "Dinaminis RAM",                                        //23
            "Dinaminiai branduoliai",                               //24
            "Aukštesnė prioriteta",                                 //25
            "Automatinis dulkės",                                   //26
            "Automatinis pažeidimas",                               //27
            "Interneto režimas",                                    //28
            "Pasirinkti žaidimą",                                   //29
            "Atverti aplanką",                                      //30
            "Ryšio pataisa",                                        //31
            "Modifikacijų naršyklė",                                //32
            "Žaidimo atnaujinimas",                                 //33
            "Išsaugoti",                                            //34
            "Žaidimo versija: ",                                     //35
            "Žaidimo kūrimo versija: {0}",                           //36
            "Žaidimo kelias: {0}",                                   //37
            "Žaidimo įdiegimo dydis: {0}",                          //38
            "Žaidimo tipas: {0} '{1}' - {2}",                        //39
            " (Neteisinga žaidimo versija) Reikalingas atnaujinimas", //40
            "Pasirinkite savo Test Drive Unlimited žaidimą",         //41
            "Konfigūracijos failas pakeistas",                      //42
            "Žaidimo katalogas neegzistuoja",                       //43
            "Katalogas neegzistuoja: ",                             //44
            "Profilis: {0} sėkmingai išsaugotas",                   //45
            "Klaida išsaugojant profilį: {0} Klaida: {1}",           //46
            //Create Profile UI-Langs end
        
            //Service UI-Langs 
            "Paskyra arba slaptažodis negali būti tuščias",        //47
            "Paskyra arba slaptažodis negali būti tuščias",        //48
            "Slaptažodis negali būti tuščias",                     //49
            "Neteisingi prisijungimo duomenys, negalima prisijungti", //50
            "Klaida prisijungiant prie debesies: {0}",              //51
            "Būsena: {0}\nSandėliavimo dydis: {1}\nPaskutinis įkėlimas: {2}\nIšsaugojimo vardas: {3}\nPaskutinis tikrinimas: {4}", //52
            "Išsaugojimas egzistuoja",                              //53
            "Nėra išsaugojimų",                                     //54
            "Klaida įkeliant išsaugojimo informaciją: {0}",         //55
            "Įkelti išsaugojimą",                                   //56
            "Įkėlimo progresas: {0}%, Dydis: {1}",                  //57  
            "Atsisiuntimo progresas: {0}%, Dydis: {1}",             //58
            "Išsaugojimas įkeltas",                                 //59
            "Atsisiųsti išsaugojimą",                               //60
            "Išsaugojimas atsisiųstas",                             //61
            "Išsaugojimo išskleidimas",                             //62
            "Laikinų failų valymas",                                //63
            "PP2 paskyros informacija",                             //64
            "Paskyros vardas",                                      //65
            "Paskyros slaptažodis",                                //66
            "Prisijungti",                                          //67
            "Atsijungti",                                           //68
            "Atnaujinti",                                           //69
            "Įkelti",                                               //70
            "Atsisiųsti",                                           //71
            "Atsarginės kopijos aplankas",                          //72
            "Išsaugojimų aplankas",                                 //73
            "Maksimalus sandėliavimo taškų skaičius",               //74
            "Išvalyti",                                             //75
            //Service UI-Langs end
        
            //Update UI-Langs
            "Paleidžiama žaidimo patikra",                          //76
            "Nepakartotinis žaidimas aptiktas, stabdomas atnaujinimas", //77
            "Nėra įprastos žaidimo įdiegties aptikta, stabdomas atnaujinimas", //78
            "Rasta failų: {0} aplankų: {1}",                        //79
            "Jungiamasi prie serverio",                             //80
            "Prisijungta prie serverio: GERAI, paleidžiama failų tikrinimas", //81
            "Nepavyko prisijungti prie serverio",                   //82
            "Palyginami failai, 1 etapas",                          //83
            "1 etapas: Tikrinami trūkstami failai",                 //84
            "Trūkstami failai: {0}",                                //85
            "Trūksta: {0} - {1}",                                   //86
            "Tikrinami failai dėl pokyčių",                         //87
            "Palyginami failai, 2 etapas",                          //88
            "2 etapas: Tikrinamas failas: ",                        //89
            "Failas: {0} ◄ Reikalingas atnaujinimas, failas skiriasi [{1}|{2}]", //90
            "Failas: {0} ◄ Failas yra naujausias",                  //91
            "Failų tikrinimas panašiai nepavyko. Bandykite dar kartą.", //92
            "Failų tikrinimas baigtas per: {0}",                    //93
            "Nepavyko gauti žaidimo failų sąrašo internete. Bandykite vėliau.", //94
            "Gyvų failų sąrašas atsisiųstas.",                      //95
            "Per daug trūkstamų failų. Atnaujinimas nutrauktas. Įdiekite žaidimą iš naujo.", //96
            "Failai ir aplankai nuskaityti.",                       //97
            "Trūkstamas katalogas sukurtas: ",                      //98
            "Palyginami failai, 3 etapas",                          //99
            "3 etapas: Failų vienties patikra",                      //100
            "3 etapas: Failų atsisiuntimas ir įdiegimas",           //101
            "Atsisiunčiamas failas: {0} ({1}% baigta)",             //102
            "Atnaujinimas baigtas per: {0}",                        //103
            "Žaidimo atnaujinimas baigtas.",                        //104
            "Failas atnaujintas: ",                                 //105
            "Nepavyko išskleisti failo: ({0}). Klaida: {1}",         //106
            "Failas: {0} ◄ Įdiegimas",                              //107
            "Žaidimo tikrinimas baigtas",                           //108
        
            //Gameupdate end
        
            //Generic Messages
            "Project Paradise 2 - Saugumo modulis",                 //109
            "Saugumo modulis nėra saugus. Žaidimas buvo uždarytas.", //110
            "Aptiktas Steam žaidimas, ar tai teisinga?",            //111
            "Aptiktas nepakartotinis žaidimas, ar tai teisinga?",    //112
            "Įspėjimas: Naudoti nepakartotinius žaidimus savo rizika!\nČia mes nematome pagalbos ar priežiūros!", //113
            "Įspėjimas: Jei tai vis dar nepakartotinis žaidimas, kai kurios veiksmų gali sugadinti žaidimą!\nIr gali vesti prie naujo įdiegimo", //114
            "Žaidimo serveriai prisijungę, žaidėjai: ",             //115
            "Klaida gaunant informaciją, spustelėkite čia norėdami atnaujinti", //116
        
            //Update UI-Langs end
        
            //Mod Detail View
            "Atgal",                                                //117
            "Kūrėjas",                                              //118
            "Aprašymas",                                            //119
            "Versija",                                              //120
            "Failai",                                               //121
            "Atsisiuntimo dydis",                                   //122
            "Įdiegimo dydis",                                       //123
            "Atnaujinimai",                                         //124
            "Įdiegtys",                                             //125
            "Įdiegti | Atnaujinti",                                //126
            "Modifikacijos pavadinimas",                            //127
            "Pradedamas modifikacijų atsisiuntimas",                //128
            "Modifikacijos įdiegimas ({0}) baigtas",               //129
            "Įdiegiamas modifikacijos ({0}) failas: {1} iš {2}",   //130
            "Įdiegtos modifikacijos",                               //131
            "Rodoma: {0} modifikacijų iš {1}",                      //132
            "Nėra modifikacijų šiam žaidimo tipui",                 //133
            "Įkeliama modifikacijos",                               //134
            "Modifikacija: {0} sėkmingai pašalinta",               //135
            "Nepakartotinės modifikacijos negali būti pašalintos iš leistuvės", //136
            "Neteisingas modifikacijos ID, pabandykite vėliau po paleidimo", //137
            "Ypač ačiū \nMūsų puikiai komandai ir mūsų patronams – jūsų parama, įsipareigojimas ir indėlis labai mums svarbus. \nNuo Discord pagalbos iki serverių veikimo – jūs padedate šiam projektui būti įgyvendinamą. Mes vertiname kiekvieną iš jūsų labai daug.", //138
            "Leistuvė turi seną duomenų bazės versiją, \nAr turėtų bandyti konvertuoti senus nustatymus į naują profilį?", //139
            "Turite sukurti savo žaidimo profilį,\nPrieš galėdami žaisti.",                                        //140
            "Prašome patikrinti profilio nustatymus\nProfilis sėkmingai sukurtas kaip: Konvertuotas profilis",      //141
        };

        public static string[] Polish =
        {
            //Main UI-Langs
            "Graj",                                                 //0
            "Start",                                                //1
            "Ustawienia",                                           //2
            "Profile",                                              //3
            "Usługa",                                               //4
            "Informacje",                                           //5
            "O programie",                                          //6
            "Zakończ",                                              //7
            "Sprawdź status",                                       //8
            "Wstecz",                                               //9
            "Autor",                                                //10
            "Opis",                                                 //11
            "Aktualizacje",                                         //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Status Discord",                                       //13
            "Sprawdzanie aktualizacji",                             //14
            "Przekazywanie portów UPnP",                           //15
            "Język",                                                //16
            "Tryb dźwięku",                                         //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Utwórz nowy profil",                                   //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Utwórz nową instancję gry",                            //19
            "Edytuj instancję gry",                                 //20
            "Nazwa profilu",                                        //21
            "Argumenty",                                   //22
            "Dynamiczna RAM",                                       //23
            "Dynamiczne rdzenie",                                   //24
            "Wyższy priorytet",                                     //25
            "Auto-Dirt",                                            //26
            "Auto-Damage",                                          //27
            "Tryb online",                                          //28
            "Wybierz grę",                                          //29
            "Otwórz folder",                                        //30
            "Patch połączenia",                                     //31
            "Przeglądarka modów",                                   //32
            "Aktualizacja gry",                                     //33
            "Zapisz",                                               //34
            "Wersja gry: ",                                         //35
            "Build gry: {0}",                                       //36
            "Ścieżka do gry: {0}",                                  //37
            "Rozmiar instalacji gry: {0}",                          //38
            "Typ gry: {0} '{1}' - {2}",                             //39
            " (Nieprawidłowa wersja gry) Wymagana aktualizacja",   //40
            "Wybierz swoją grę Test Drive Unlimited",                //41
            "Plik konfiguracyjny został zamieniony",               //42
            "Folder gry nie istnieje.",                             //43
            "Folder nie istnieje: ",                                //44
            "Profil: {0} został pomyślnie zapisany",                //45
            "Błąd podczas zapisywania profilu: {0} Błąd: {1}",      //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Konto lub hasło nie może być puste",                  //47
            "Konto lub hasło nie może być puste",                  //48
            "Hasło nie może być puste",                             //49
            "Nieprawidłowe dane logowania, nie można się zalogować", //50
            "Błąd logowania do chmury: {0}",                        //51
            "Status: {0}\nPojemność dysku: {1}\nOstatnie przesłanie: {2}\nNazwa zapisu: {3}\nOstatnia weryfikacja: {4}", //52
            "Zapis istnieje",                                       //53
            "Brak zapisów",                                         //54
            "Błąd ładowania informacji o zapisie: {0}",             //55
            "Przesyłanie zapisu",                                   //56
            "Postęp przesyłania: {0}%, Rozmiar: {1}",              //57  
            "Postęp pobierania: {0}%, Rozmiar: {1}",               //58
            "Zapis przesłany",                                      //59
            "Pobieranie zapisu",                                    //60
            "Zapis pobrany",                                        //61
            "Rozpakowywanie zapisu",                                //62
            "Czyszczenie plików tymczasowych",                      //63
            "Informacje o koncie PP2",                              //64
            "Nazwa konta",                                          //65
            "Hasło konta",                                          //66
            "Zaloguj się",                                          //67
            "Wyloguj się",                                          //68
            "Odśwież",                                              //69
            "Przesyłanie",                                          //70
            "Pobieranie",                                           //71
            "Folder kopii zapasowej",                               //72
            "Folder zapisów",                                       //73
            "Maksymalna pojemność dysku",                           //74
            "Wyczyść",                                              //75
            //Service UI-Langs end
            //Update UI-Langs
            "Uruchamianie sprawdzania gry",                         //76
            "Wykryto rozpakowaną grę, zatrzymaj aktualizację",      //77
            "Nie wykryto normalnej instalacji gry, zatrzymaj aktualizację", //78
            "Znalezione pliki: {0} Foldery: {1}",                  //79
            "Łączenie z serwerem",                                  //80
            "Połączenie z serwerem: OK, uruchamianie sprawdzania plików", //81
            "Nie można połączyć się z serwerem",                    //82
            "Porównywanie plików, faza 1",                          //83
            "Faza 1: Sprawdzanie brakujących plików",               //84
            "Brakujące pliki: {0}",                                 //85
            "Brakuje: {0} - {1}",                                   //86
            "Sprawdzanie plików pod kątem zmian",                   //87
            "Porównywanie plików, faza 2",                          //88
            "Faza 2: Sprawdzanie pliku: ",                          //89
            "Plik: {0} ◄ Wymagana aktualizacja, plik różni się [{1}|{2}]", //90
            "Plik: {0} ◄ Plik jest aktualny",                       //91
            "Sprawdzanie plików nie powiodło się. Spróbuj ponownie.", //92
            "Sprawdzanie plików zakończone w: {0}",                 //93
            "Nie można pobrać listy plików online. Spróbuj ponownie później.", //94
            "Pobrano listę plików online.",                         //95
            "Zbyt wiele brakujących plików. Aktualizacja została przerwana. Zainstaluj grę ponownie.", //96
            "Przeskanowano pliki i foldery.",                       //97
            "Utworzono brakujący katalog: ",                        //98
            "Porównywanie plików, faza 3",                          //99
            "Faza 3: Sprawdzanie integralności plików",             //100
            "Faza 3: Pobieranie i instalowanie plików",             //101
            "Pobieranie pliku: {0} ({1}% ukończone)",              //102
            "Aktualizacja zakończona w: {0}",                       //103
            "Aktualizacja gry zakończona.",                         //104
            "Plik zaktualizowany: ",                                //105
            "Błąd rozpakowania pliku ({0}). Błąd: {1}",             //106
            "Plik: {0} ◄ Instalowanie",                             //107
            "Sprawdzanie gry zakończone",                           //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Moduł bezpieczeństwa",           //109
            "Moduł bezpieczeństwa jest niebezpieczny. Gra została zamknięta.", //110
            "Wykryto grę Steam, czy to poprawne?",                 //111
            "Wykryto rozpakowaną grę, czy to poprawne?",            //112
            "Ostrzeżenie: Używanie rozpakowanych gier jest na własne ryzyko!\nNie udzielamy pomocy ani wsparcia!", //113
            "Ostrzeżenie: Jeśli to wciąż jest rozpakowana gra, może się zdarzyć, że niektóre działania zniszczą grę!\nMoże również prowadzić do reinstalacji", //114
            "Serwery gier online, gracze: ",                        //115
            "Błąd podczas pobierania informacji, kliknij tutaj, aby odświeżyć", //116
            //Update UI-Langs end
            //Mod Detail View
            "Powrót",                                               //117
            "Autor",                                                //118
            "Opis",                                                 //119
            "Wersja",                                               //120
            "Pliki",                                                //121
            "Rozmiar pobierania",                                   //122
            "Rozmiar instalacji",                                   //123
            "Aktualizacje",                                         //124
            "Instalacje",                                           //125
            "Zainstaluj | Zaktualizuj",                            //126
            "Nazwa moda",                                           //127
            "Rozpoczynanie pobierania moda",                        //128
            "Instalacja moda ({0}) zakończona",                     //129
            "Instalowanie moda ({0}) plik: {1} z {2}",              //130
            "Zainstalowane mody",                                   //131
            "Wyświetlanie: {0} modów z {1}",                        //132
            "Brak dostępnych modów dla tego typu gry",              //133
            "Wczytywanie modów",                                    //134
            "Mod: {0} został pomyślnie odinstalowany",              //135
            "Mody rozpakowane nie mogą być odinstalowane przez launcher.", //136
            "Nieprawidłowy identyfikator moda, spróbuj ponownie po restarcie", //137
            "Specjalne podziękowania \nDla naszego wspaniałego zespołu i patronów – Twoja pomoc, zaangażowanie i wsparcie znaczą dla nas bardzo. \nOd pomocy na Discordzie po utrzymanie serwerów – wasze działania sprawiają, że ten projekt jest możliwy. Czcimy każdego z was.", //138
            "Launcher ma starą wersję bazy danych,\nCzy spróbować skonwertować stare ustawienia do nowego profilu?", //139
            "Musisz samodzielnie utworzyć profil gry,\nZanim będziesz mógł grać.",                                  //140
            "Proszę sprawdzić ustawienia profilu\nProfil został pomyślnie utworzony jako: Skonwertowany Profil",       //141
        };

        public static string[] Russian =
        {
            //Main UI-Langs
            "Играть",                                              //0
            "Старт",                                               //1
            "Настройки",                                           //2
            "Профили",                                             //3
            "Сервис",                                              //4
            "Информация",                                          //5
            "О программе",                                         //6
            "Выход",                                               //7
            "Проверить статус",                                    //8
            "Назад",                                               //9
            "Автор",                                               //10
            "Описание",                                            //11
            "Обновления",                                          //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Статус Discord",                                      //13
            "Проверка обновлений",                                 //14
            "UPnP переадресация портов",                           //15
            "Язык",                                                //16
            "Режим звука",                                         //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Создать новый профиль",                               //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Создать новую игровую инстанцию",                     //19
            "Редактировать игровую инстанцию",                     //20
            "Имя профиля",                                         //21
            "аргументы",                                 //22
            "Динамическая память (RAM)",                           //23
            "Динамические ядра",                                   //24
            "Высокий приоритет",                                   //25
            "Авто-грязь",                                          //26
            "Авто-повреждения",                                    //27
            "Онлайн-режим",                                        //28
            "Выбрать игру",                                        //29
            "Открыть папку",                                       //30
            "Патч соединения",                                     //31
            "Мод-браузер",                                         //32
            "Обновление игры",                                     //33
            "Сохранить",                                           //34
            "Версия игры: ",                                       //35
            "Сборка игры: {0}",                                    //36
            "Путь к игре: {0}",                                    //37
            "Размер установки: {0}",                               //38
            "Тип игры: {0} '{1}' - {2}",                           //39
            " (Неправильная версия игры) Требуется обновление",    //40
            "Выберите свою игру Test Drive Unlimited",             //41
            "Файл конфигурации был заменён",                       //42
            "Каталог игры не существует.",                         //43
            "Папка не существует: ",                               //44
            "Профиль: {0} успешно сохранён",                       //45
            "Ошибка при сохранении профиля: {0} Ошибка: {1}",      //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Аккаунт или пароль не могут быть пустыми",            //47
            "Аккаунт или пароль не могут быть пустыми",            //48
            "Пароль не может быть пустым",                         //49
            "Неверные данные входа, вход невозможен",              //50
            "Ошибка входа в облако: {0}",                          //51
            "Статус: {0}\nРазмер: {1}\nПоследняя загрузка: {2}\nИмя сохранения: {3}\nПоследняя проверка: {4}", //52
            "Сохранение существует",                               //53
            "Сохранение не найдено",                               //54
            "Ошибка при загрузке информации о сохранении: {0}",    //55
            "Загрузить сохранение",                                //56
            "Прогресс загрузки: {0}%, размер: {1}",                //57  
            "Прогресс скачивания: {0}%, размер: {1}",              //58
            "Сохранение загружено",                                //59
            "Скачать сохранение",                                  //60
            "Сохранение скачано",                                  //61
            "Распаковать сохранение",                              //62
            "Очистка временных файлов",                            //63
            "Информация об аккаунте PP2",                          //64
            "Имя аккаунта",                                        //65
            "Пароль аккаунта",                                     //66
            "Войти",                                               //67
            "Выйти",                                               //68
            "Обновить",                                            //69
            "Загрузить",                                           //70
            "Скачать",                                             //71
            "Папка резервных копий",                               //72
            "Папка сохранений",                                    //73
            "Макс. количество точек сохранения",                   //74
            "Очистить",                                            //75
            //Service UI-Langs end
            //Update UI-Langs
            "Запуск проверки игры",                                //76
            "Обнаружена распакованная версия игры, обновление остановлено", //77
            "Обычная установка игры не найдена, обновление остановлено", //78
            "Файлы: {0} Папки: {1}",                               //79
            "Подключение к серверу",                               //80
            "Соединение с сервером: OK, начинаю проверку файлов",  //81
            "Не удалось подключиться к серверу",                   //82
            "Сравнение файлов, фаза 1",                            //83
            "Фаза 1: проверка на отсутствие файлов",               //84
            "Отсутствующие файлы: {0}",                            //85
            "Отсутствует: {0} - {1}",                              //86
            "Проверка изменений в файлах",                         //87
            "Сравнение файлов, фаза 2",                            //88
            "Фаза 2: проверяется файл: ",                          //89
            "Файл: {0} ◄ Требуется обновление, различие [{1}|{2}]", //90
            "Файл: {0} ◄ Файл актуален",                           //91
            "Проверка файлов не удалась. Попробуйте снова.",       //92
            "Проверка завершена за: {0}",                          //93
            "Не удалось получить онлайн-список файлов. Повторите попытку позже.", //94
            "Список файлов загружен.",                             //95
            "Слишком много отсутствующих файлов. Обновление отменено. Переустановите игру.", //96
            "Файлы и папки просканированы.",                       //97
            "Создан отсутствующий каталог: ",                      //98
            "Сравнение файлов, фаза 3",                            //99
            "Фаза 3: проверка целостности файлов",                 //100
            "Фаза 3: загрузка и установка файлов",                 //101
            "Загрузка файла: {0} ({1}% завершено)",                //102
            "Обновление завершено за: {0}",                        //103
            "Обновление игры завершено.",                          //104
            "Файл обновлён: ",                                     //105
            "Ошибка при распаковке файла: ({0}). Ошибка: {1}",     //106
            "Файл: {0} ◄ Устанавливается",                         //107
            "Проверка игры завершена",                             //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Модуль безопасности",             //109
            "Модуль безопасности небезопасен. Игра закрыта.",       //110
            "Обнаружена Steam-версия игры, это верно?",             //111
            "Обнаружена распакованная версия игры, это верно?",     //112
            "Предупреждение: использование распакованных версий на свой страх и риск!\nПоддержка не предоставляется!", //113
            "Предупреждение: если это всё же распакованная версия, некоторые действия могут повредить игру\nи привести к переустановке!", //114
            "Игровые серверы онлайн, игроков: ",                    //115
            "Ошибка при получении информации. Нажмите здесь, чтобы обновить", //116
            //Update UI-Langs end
            //Mod Detail View
            "Назад",                                                //117
            "Автор",                                                //118
            "Описание",                                             //119
            "Версия",                                               //120
            "Файлы",                                                //121
            "Размер загрузки",                                      //122
            "Размер установки",                                     //123
            "Обновления",                                           //124
            "Установки",                                            //125
            "Установить | Обновить",                                //126
            "Имя мода",                                             //127
            "Начало загрузки мода",                                //128
            "Установка мода ({0}) завершена",                       //129
            "Установка мода ({0}) файл: {1} из {2}",                //130
            "Установленные моды",                                   //131
            "Показано: {0} модов из {1}",                           //132
            "Нет доступных модов для этого типа игры",              //133
            "Загрузка модов",                                       //134
            "Мод: {0} успешно удалён",                              //135
            "Распакованные моды нельзя удалить через лаунчер.",     //136
            "Неверный ModId, попробуйте после перезапуска",         //137
            "Особая благодарность \nНашей замечательной команде и нашим патронам – за вашу поддержку, участие и вклад.\nОт помощи в Discord до работы серверов – вы делаете этот проект возможным. Мы очень ценим каждого из вас.", //138
            "У лаунчера устаревшая версия базы данных.\nПреобразовать старые настройки в новый профиль?", //139
            "Сначала создайте игровой профиль,\nпрежде чем начинать игру.", //140
            "Проверьте настройки профиля.\nПрофиль успешно создан как: Converted Profile", //141
        };

        public static string[] Portuguese_Brazil =
        {
            //Main UI-Langs
            "Jogar",                                                //0
            "Iniciar",                                              //1
            "Configurações",                                        //2
            "Perfis",                                               //3
            "Serviço",                                              //4
            "Informações",                                          //5
            "Sobre",                                                //6
            "Sair",                                                 //7
            "Verificar Status",                                     //8
            "Voltar",                                               //9
            "Criador",                                              //10
            "Descrição",                                            //11
            "Atualizações",                                         //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Status do Discord",                                    //13
            "Verificação de Atualização",                           //14
            "Redirecionamento de Porta UPnP",                       //15
            "Idioma",                                               //16
            "Modo de Áudio",                                        //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Criar Novo Perfil",                                    //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Criar Nova Instância do Jogo",                         //19
            "Editar Instância do Jogo",                             //20
            "Nome do Perfil",                                       //21
            "Argumentos",                          //22
            "RAM Dinâmico",                                         //23
            "Núcleos Dinâmicos",                                    //24
            "Prioridade Alta",                                      //25
            "Auto-Dirt",                                            //26
            "Auto-Dano",                                            //27
            "Modo Online",                                          //28
            "Selecionar Jogo",                                      //29
            "Abrir Pasta",                                          //30
            "Patch de Conexão",                                     //31
            "Navegador de Mods",                                    //32
            "Atualização do Jogo",                                  //33
            "Salvar",                                               //34
            "Versão do Jogo: ",                                     //35
            "Build do Jogo: {0}",                                   //36
            "Caminho do Jogo: {0}",                                 //37
            "Tamanho da Instalação do Jogo: {0}",                   //38
            "Tipo de Jogo: {0} '{1}' - {2}",                        //39
            " (Versão Incorreta) Atualização Necessária",           //40
            "Escolha seu Jogo Test Drive Unlimited",                //41
            "Arquivo de Configuração Trocado",                      //42
            "Pasta do Jogo Não Existe.",                            //43
            "Pasta Não Existe: ",                                   //44
            "Perfil: {0} Salvo com Sucesso",                        //45
            "Erro ao salvar o perfil: {0} Erro: {1}",               //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Conta ou Senha não pode estar vazio",                  //47
            "Conta ou Senha não pode estar vazio",                  //48
            "Senha não pode estar vazia",                           //49
            "Credenciais Inválidas, Não é Possível Entrar",         //50
            "Erro ao Entrar na Nuvem: {0}",                         //51
            "Status: {0}\nTamanho do Armazenamento: {1}\nÚltimo Upload: {2}\nNome do Savegame: {3}\nÚltima Verificação: {4}", //52
            "Savegame Existe",                                      //53
            "Nenhum Savegame Existe",                               //54
            "Erro ao Carregar Informações do Savegame: {0}",       //55
            "Enviar Savegame",                                      //56
            "Progresso do Upload: {0}%, Tamanho: {1}",              //57  
            "Progresso do Download: {0}%, Tamanho: {1}",            //58
            "Savegame Enviado",                                     //59
            "Baixar Savegame",                                      //60
            "Savegame Baixado",                                     //61
            "Desempacotando Savegame",                              //62
            "Limpeza de Arquivos Temporários",                      //63
            "Informações da Conta PP2",                             //64
            "Nome da Conta",                                        //65
            "Senha da Conta",                                       //66
            "Entrar",                                               //67
            "Sair",                                                 //68
            "Atualizar",                                            //69
            "Enviar",                                               //70
            "Baixar",                                               //71
            "Pasta de Backup",                                      //72
            "Pasta de Savegames",                                   //73
            "Máximo de Pontos de Armazenamento",                    //74
            "Limpar",                                               //75
            //Service UI-Langs end
            //Update UI-Langs
            "Iniciando Verificação do Jogo",                        //76
            "Jogo Desempacotado Detectado, Parando Atualização",    //77
            "Nenhuma Instalação Normal de Jogo Detectada, Parando Atualização", //78
            "Arquivos Encontrados: {0} Pastas: {1}",               //79
            "Conectando ao Servidor",                               //80
            "Conexão com o Servidor OK, Iniciando Verificação de Arquivos", //81
            "Não foi possível conectar ao servidor",                //82
            "Comparando Arquivos, Fase 1",                          //83
            "Fase 1: Verificando Arquivos Faltando",                //84
            "Arquivos Faltando: {0}",                               //85
            "Faltando: {0} - {1}",                                  //86
            "Verificando Arquivos por Alterações",                  //87
            "Comparando Arquivos, Fase 2",                          //88
            "Fase 2: Verificando Arquivo: ",                        //89
            "Arquivo: {0} ◄ Atualização Necessária, Arquivo Diferente [{1}|{2}]", //90
            "Arquivo: {0} ◄ Arquivo está Atualizado",               //91
            "A verificação de arquivos parece ter falhado. Por favor, tente novamente.", //92
            "Verificação de Arquivos Concluída em: {0}",            //93
            "Não foi possível obter a lista de arquivos online. Tente novamente mais tarde.", //94
            "Lista de Arquivos Online Baixada.",                    //95
            "Muitos Arquivos Faltando. Atualização Cancelada. Por favor, reinstale o jogo.", //96
            "Arquivos e Pastas Escaneados.",                        //97
            "Diretório Faltante Criado: ",                         //98
            "Comparando Arquivos, Fase 3",                          //99
            "Fase 3: Verificando Integridade dos Arquivos",         //100
            "Fase 3: Baixando e Instalando Arquivos",               //101
            "Baixando Arquivo: {0} ({1}% Concluído)",              //102
            "Atualização Concluída em: {0}",                        //103
            "Atualização do Jogo Finalizada.",                      //104
            "Arquivo Atualizado: ",                                 //105
            "Falha ao Desempacotar Arquivo: ({0}). Erro: {1}",      //106
            "Arquivo: {0} ◄ Instalando",                            //107
            "Verificação do Jogo Concluída",                        //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Módulo de Segurança",             //109
            "O módulo de segurança não é seguro. O jogo foi fechado.", //110
            "Um jogo Steam foi detectado, está correto?",          //111
            "Um jogo desempacotado foi detectado, está correto?",   //112
            "Aviso: Usar jogos desempacotados é por sua conta e risco!\nNão oferecemos ajuda ou suporte aqui!", //113
            "Aviso: Se ainda for um jogo desempacotado, algumas ações podem corromper o jogo!\nE pode exigir reinstalação.", //114
            "Servidores de Jogo Online, Jogadores: ",              //115
            "Erro ao buscar informações, clique aqui para atualizar", //116
            //Update UI-Langs end
            //Mod Detail View
            "Voltar",                                               //117
            "Criador",                                              //118
            "Descrição",                                            //119
            "Versão",                                               //120
            "Arquivos",                                             //121
            "Tamanho do Download",                                  //122
            "Tamanho da Instalação",                                //123
            "Atualizações",                                         //124
            "Instalações",                                          //125
            "Instalar | Atualizar",                                 //126
            "Nome do Mod",                                          //127
            "Iniciando Download do Mod",                            //128
            "Instalação do Mod ({0}) Concluída",                    //129
            "Instalando Mod ({0}) Arquivo: {1} de {2}",             //130
            "Mods Instalados",                                      //131
            "Mostrando: {0} Mods de {1}",                           //132
            "Nenhum Mod Disponível para Este Tipo de Jogo",         //133
            "Carregando Mods",                                      //134
            "Mod: {0} foi desinstalado com sucesso",                //135
            "Mods Desempacotados não podem ser desinstalados pelo Launcher.", //136
            "ID de Mod Inválido. Tente reiniciar o programa.",      //137
            "Um agradecimento especial \nAo nosso incrível time e nossos patreons – seu apoio, engajamento e contribuições significam muito para nós. \nDesde ajuda no Discord até operação dos servidores – vocês fazem este projeto possível. Valorizamos cada um de vocês.", //138
            "O launcher possui uma versão antiga da base de dados,\nDeseja tentar converter as configurações antigas para um novo perfil?", //139
            "Você precisa criar um perfil de jogo,\nAntes de jogar.", //140
            "Por favor, verifique as configurações do perfil\nO perfil foi criado com sucesso como: Perfil Convertido", //141
        };

        public static string[] English_UK =
        {
            //Main UI-Langs
            "Play",                                                 //0
            "Start",                                                //1
            "Settings",                                             //2
            "Profiles",                                             //3
            "Service",                                              //4
            "Information",                                          //5
            "About",                                                //6
            "Exit",                                                 //7
            "Check Status",                                         //8
            "Back",                                                 //9
            "Creator",                                              //10
            "Description",                                          //11
            "Updates",                                              //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Discord Status",                                       //13
            "Update Check",                                         //14
            "UPnP Port Forwarding",                                 //15
            "Language",                                             //16
            "Sound Mode",                                           //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Create New Profile",                                   //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Create New Game Instance",                             //19
            "Edit Game Instance",                                   //20
            "Profile Name",                                         //21
            "Launch Args",                                          //22
            "Dynamic RAM",                                          //23
            "Dynamic Cores",                                        //24
            "Higher Priority",                                      //25
            "Auto Dirt",                                            //26
            "Auto Damage",                                          //27
            "Online Mode",                                          //28
            "Select Game",                                          //29
            "Open Folder",                                          //30
            "Connection Patch",                                     //31
            "Mod Browser",                                          //32
            "Game Update",                                          //33
            "Save",                                                 //34
            "Game Version: ",                                       //35
            "Game Build: {0}",                                      //36
            "Game Path: {0}",                                       //37
            "Installation Size: {0}",                               //38
            "Game Type: {0} '{1}' - {2}",                           //39
            " (Incorrect Game Version) Update Required",            //40
            "Select your Test Drive Unlimited game",                //41
            "Config file has been replaced",                        //42
            "Game directory does not exist.",                       //43
            "Folder does not exist: ",                              //44
            "Profile: {0} saved successfully",                      //45
            "Error saving profile: {0} Error: {1}",                 //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Account or password cannot be empty",                  //47
            "Account or password cannot be empty",                  //48
            "Password cannot be empty",                             //49
            "Incorrect login details, login not possible",          //50
            "Error logging into cloud: {0}",                        //51
            "Status: {0}\nStorage Size: {1}\nLast Uploaded: {2}\nSavegame Name: {3}\nLast Checked: {4}", //52
            "Savegame Exists",                                      //53
            "No Savegame Exists",                                   //54
            "Error loading savegame info: {0}",                     //55
            "Upload Savegame",                                      //56
            "Upload Progress: {0}%, Size: {1}",                     //57  
            "Download Progress: {0}%, Size: {1}",                   //58
            "Savegame Uploaded",                                    //59
            "Download Savegame",                                    //60
            "Savegame Downloaded",                                  //61
            "Extract Savegame",                                     //62
            "Cleanup Temp Files",                                   //63
            "PP2 Account Info",                                     //64
            "Account Name",                                         //65
            "Account Password",                                     //66
            "Login",                                                //67
            "Logout",                                               //68
            "Refresh",                                              //69
            "Upload",                                               //70
            "Download",                                             //71
            "Backup Folder",                                        //72
            "Savegame Folder",                                      //73
            "Max. Save Points",                                     //74
            "Cleanup",                                              //75
            //Service UI-Langs end
            //Update UI-Langs
            "Starting Game Verification",                           //76
            "Unpacked Game Detected, Stopping Update",              //77
            "No Regular Game Installation Detected, Stopping Update", //78
            "Files Found: {0} Folders: {1}",                        //79
            "Connecting to Server",                                 //80
            "Connection to Server: OK, Starting File Check",        //81
            "Could Not Connect to Server",                          //82
            "Comparing Files, Phase 1",                             //83
            "Phase 1: Checking for Missing Files",                  //84
            "Missing Files: {0}",                                   //85
            "Missing: {0} - {1}",                                   //86
            "Checking Files for Modifications",                     //87
            "Comparing Files, Phase 2",                             //88
            "Phase 2: Checking File: ",                             //89
            "File: {0} ◄ Update Required, File Differs [{1}|{2}]",  //90
            "File: {0} ◄ File is Up-to-Date",                       //91
            "File Verification Failed. Please Try Again.",          //92
            "File Verification Completed in: {0}",                  //93
            "Could Not Retrieve Online File List. Please Try Again Later.", //94
            "Live File List Downloaded.",                           //95
            "Too Many Missing Files. Update Aborted. Please Reinstall the Game.", //96
            "Files and Folders Scanned.",                           //97
            "Created Missing Directory: ",                          //98
            "Comparing Files, Phase 3",                             //99
            "Phase 3: Verifying File Integrity",                    //100
            "Phase 3: Downloading and Installing Files",            //101
            "Downloading File: {0} ({1}% Complete)",                //102
            "Update Completed in: {0}",                             //103
            "Game Update Finished.",                                //104
            "File Updated: ",                                       //105
            "Failed to Extract File: ({0}). Error: {1}",            //106
            "File: {0} ◄ Installing",                               //107
            "Game Check Complete",                                  //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Security Module",                 //109
            "The security module is not secure. The game has been closed.", //110
            "A Steam version of the game was detected. Is that correct?", //111
            "An unpacked version of the game was detected. Is that correct?", //112
            "Warning: Using unpacked games is at your own risk!\nWe provide no help or support for this!", //113
            "Warning: If this is indeed an unpacked game, some actions may damage your installation\nand require reinstallation!", //114
            "Game Servers Online, Players: ",                       //115
            "Error retrieving information. Click here to refresh",  //116
            //Update UI-Langs end
            //Mod Detail View
            "Back",                                                 //117
            "Creator",                                              //118
            "Description",                                          //119
            "Version",                                              //120
            "Files",                                                //121
            "Download Size",                                        //122
            "Installation Size",                                    //123
            "Updates",                                              //124
            "Installations",                                        //125
            "Install | Update",                                     //126
            "Mod Name",                                             //127
            "Starting Mod Download",                                //128
            "Mod Installation ({0}) Completed",                     //129
            "Installing Mod ({0}) File: {1} of {2}",                //130
            "Installed Mods",                                       //131
            "Showing: {0} Mods of {1}",                             //132
            "No Mods Available for this Game Type",                 //133
            "Loading Mods",                                         //134
            "Mod: {0} Uninstalled Successfully",                    //135
            "Unpacked Mods Cannot Be Uninstalled via Launcher.",    //136
            "Invalid ModId. Please Try Again After Restart",        //137
            "A very special thank you \nto our wonderful team and patrons – your support, dedication and contributions mean the world to us. \nFrom helping on Discord to keeping the servers running – you make this project possible. We truly appreciate each and every one of you.", //138
            "The launcher has an outdated database version.\nWould you like to convert the old settings into a new profile?", //139
            "You must create a game profile\nbefore you can play.", //140
            "Please check the profile settings.\nProfile successfully created as: Converted Profile", //141
        };

        public static string[] Dutch =
        {
            //Main UI-Langs
            "Spelen",                                               //0
            "Start",                                                //1
            "Instellingen",                                         //2
            "Profielen",                                            //3
            "Service",                                              //4
            "Informatie",                                           //5
            "Over",                                                 //6
            "Afsluiten",                                            //7
            "Status controleren",                                   //8
            "Terug",                                                //9
            "Maker",                                                //10
            "Beschrijving",                                         //11
            "Updates",                                              //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Discord-status",                                       //13
            "Updatecontrole",                                       //14
            "UPnP Poortdoorsturing",                                //15
            "Taal",                                                 //16
            "Geluidsmodus",                                         //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Nieuw profiel aanmaken",                               //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Nieuwe game-instantie maken",                          //19
            "Game-instantie bewerken",                              //20
            "Profielnaam",                                          //21
            "Startargs",                                      //22
            "Dynamisch RAM-gebruik",                                //23
            "Dynamische cores",                                     //24
            "Hogere prioriteit",                                    //25
            "Auto-vuil",                                            //26
            "Auto-schade",                                          //27
            "Online-modus",                                         //28
            "Spel selecteren",                                      //29
            "Map openen",                                           //30
            "Verbindingspatch",                                     //31
            "Mod-browser",                                          //32
            "Game-update",                                          //33
            "Opslaan",                                              //34
            "Gameversie: ",                                         //35
            "Gamebuild: {0}",                                       //36
            "Gamepad: {0}",                                         //37
            "Installatiegrootte: {0}",                              //38
            "Speltype: {0} '{1}' - {2}",                            //39
            " (Verkeerde gameversie) Update vereist",               //40
            "Selecteer je Test Drive Unlimited-spel",               //41
            "Configuratiebestand is vervangen",                     //42
            "Spelmap bestaat niet.",                                //43
            "Map bestaat niet: ",                                   //44
            "Profiel: {0} succesvol opgeslagen",                    //45
            "Fout bij het opslaan van profiel: {0} Fout: {1}",      //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Account of wachtwoord mag niet leeg zijn",             //47
            "Account of wachtwoord mag niet leeg zijn",             //48
            "Wachtwoord mag niet leeg zijn",                        //49
            "Onjuiste inloggegevens, inloggen niet mogelijk",       //50
            "Fout bij het inloggen op de cloud: {0}",               //51
            "Status: {0}\nOpslaggrootte: {1}\nLaatst geüpload: {2}\nSavegame-naam: {3}\nLaatste controle: {4}", //52
            "Savegame bestaat",                                     //53
            "Geen savegame gevonden",                               //54
            "Fout bij het laden van savegame-info: {0}",            //55
            "Savegame uploaden",                                    //56
            "Uploadvoortgang: {0}%, Grootte: {1}",                  //57  
            "Downloadvoortgang: {0}%, Grootte: {1}",                //58
            "Savegame geüpload",                                    //59
            "Savegame downloaden",                                  //60
            "Savegame gedownload",                                  //61
            "Savegame uitpakken",                                   //62
            "Tijdelijke bestanden opruimen",                        //63
            "PP2-accountinformatie",                                //64
            "Accountnaam",                                          //65
            "Accountwachtwoord",                                    //66
            "Inloggen",                                             //67
            "Uitloggen",                                            //68
            "Vernieuwen",                                           //69
            "Uploaden",                                             //70
            "Downloaden",                                           //71
            "Back-upmap",                                           //72
            "Savegame-map",                                         //73
            "Max. aantal opslagpunten",                             //74
            "Opruimen",                                             //75
            //Service UI-Langs end
            //Update UI-Langs
            "Gamecontrole starten",                                 //76
            "Uitgepakte game gedetecteerd, update gestopt",         //77
            "Geen normale game-installatie gedetecteerd, update gestopt", //78
            "Gevonden bestanden: {0} Mappen: {1}",                  //79
            "Verbinding maken met server",                          //80
            "Verbinding met server: OK, start bestandscontrole",    //81
            "Kon geen verbinding maken met server",                 //82
            "Bestanden vergelijken, fase 1",                        //83
            "Fase 1: controle op ontbrekende bestanden",            //84
            "Ontbrekende bestanden: {0}",                           //85
            "Ontbreekt: {0} - {1}",                                 //86
            "Bestanden controleren op wijzigingen",                 //87
            "Bestanden vergelijken, fase 2",                        //88
            "Fase 2: bestand controleren: ",                        //89
            "Bestand: {0} ◄ Update vereist, bestand verschilt [{1}|{2}]", //90
            "Bestand: {0} ◄ Bestand is up-to-date",                 //91
            "Bestandscontrole mislukt. Probeer het opnieuw.",       //92
            "Bestandscontrole voltooid in: {0}",                    //93
            "Kon online bestandslijst niet ophalen. Probeer het later opnieuw.", //94
            "Live-bestandslijst gedownload.",                       //95
            "Te veel ontbrekende bestanden. Update geannuleerd. Installeer het spel opnieuw.", //96
            "Bestanden en mappen gescand.",                         //97
            "Ontbrekende map aangemaakt: ",                         //98
            "Bestanden vergelijken, fase 3",                        //99
            "Fase 3: bestandsintegriteit controleren",              //100
            "Fase 3: bestanden downloaden en installeren",          //101
            "Bestand downloaden: {0} ({1}% voltooid)",              //102
            "Update voltooid in: {0}",                              //103
            "Game-update voltooid.",                                //104
            "Bestand bijgewerkt: ",                                 //105
            "Uitpakken van bestand mislukt: ({0}). Fout: {1}",      //106
            "Bestand: {0} ◄ Wordt geïnstalleerd",                  //107
            "Gamecontrole voltooid",                                //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Beveiligingsmodule",              //109
            "De beveiligingsmodule is niet veilig. Het spel is afgesloten.", //110
            "Een Steam-versie van het spel is gedetecteerd. Klopt dat?", //111
            "Een uitgepakte versie van het spel is gedetecteerd. Klopt dat?", //112
            "Waarschuwing: het gebruik van uitgepakte spellen is op eigen risico!\nWij bieden hiervoor geen hulp of ondersteuning!", //113
            "Waarschuwing: als dit inderdaad een uitgepakte game is, kunnen sommige acties het spel beschadigen\nof een herinstallatie vereisen!", //114
            "Game-servers online, spelers: ",                       //115
            "Fout bij het ophalen van informatie. Klik hier om te vernieuwen", //116
            //Update UI-Langs end
            //Mod Detail View
            "Terug",                                                //117
            "Maker",                                                //118
            "Beschrijving",                                         //119
            "Versie",                                               //120
            "Bestanden",                                            //121
            "Downloadgrootte",                                      //122
            "Installatiegrootte",                                   //123
            "Updates",                                              //124
            "Installaties",                                         //125
            "Installeren | Updaten",                                //126
            "Modnaam",                                              //127
            "Mod-download starten",                                 //128
            "Mod-installatie ({0}) voltooid",                       //129
            "Mod ({0}) installeren bestand: {1} van {2}",           //130
            "Geïnstalleerde mods",                                  //131
            "Toon: {0} mods van {1}",                               //132
            "Geen mods beschikbaar voor dit speltype",              //133
            "Mods laden",                                           //134
            "Mod: {0} succesvol verwijderd",                        //135
            "Uitgepakte mods kunnen niet via de launcher worden verwijderd.", //136
            "Ongeldige ModId, probeer opnieuw na herstart",         //137
            "Een speciale dank \naan ons geweldige team en onze patrons – jullie steun, inzet en bijdragen betekenen enorm veel voor ons. \nVan hulp op Discord tot het draaien van de servers – jullie maken dit project mogelijk. We waarderen jullie allemaal enorm.", //138
            "De launcher heeft een verouderde databaseversie.\nWil je de oude instellingen omzetten naar een nieuw profiel?", //139
            "Je moet eerst een spelprofiel aanmaken\nvoordat je kunt spelen.", //140
            "Controleer de profielinstellingen.\nProfiel succesvol aangemaakt als: Converted Profile", //141
        };

        public static string[] Romanian =
        {
            //Main UI-Langs
            "Joacă",                                                //0
            "Start",                                                //1
            "Setări",                                               //2
            "Profiluri",                                            //3
            "Serviciu",                                             //4
            "Informații",                                           //5
            "Despre",                                               //6
            "Ieșire",                                               //7
            "Verifică statusul",                                    //8
            "Înapoi",                                               //9
            "Autor",                                                //10
            "Descriere",                                            //11
            "Actualizări",                                          //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Status Discord",                                       //13
            "Verificare actualizări",                               //14
            "Redirecționare port UPnP",                             //15
            "Limbă",                                                //16
            "Mod sunet",                                            //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Creează un profil nou",                                //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Creează o nouă instanță de joc",                       //19
            "Editează instanța de joc",                             //20
            "Nume profil",                                          //21
            "Argumente",                                 //22
            "Utilizare dinamică a RAM-ului",                        //23
            "Procesoare dinamice",                                  //24
            "Prioritate ridicată",                                  //25
            "Murdărie automată",                                    //26
            "Daune automate",                                       //27
            "Mod online",                                           //28
            "Selectează jocul",                                     //29
            "Deschide folderul",                                    //30
            "Patch de conexiune",                                   //31
            "Browser de moduri",                                    //32
            "Actualizare joc",                                      //33
            "Salvează",                                             //34
            "Versiunea jocului: ",                                  //35
            "Build joc: {0}",                                       //36
            "Gamepad: {0}",                                         //37
            "Dimensiune instalare: {0}",                            //38
            "Tip joc: {0} '{1}' - {2}",                             //39
            " (Versiune greșită a jocului) Este necesară actualizarea", //40
            "Selectează jocul tău Test Drive Unlimited",            //41
            "Fișierul de configurare a fost înlocuit",              //42
            "Folderul jocului nu există.",                          //43
            "Folder inexistent: ",                                  //44
            "Profil: {0} salvat cu succes",                         //45
            "Eroare la salvarea profilului: {0} Eroare: {1}",       //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Contul sau parola nu pot fi goale",                    //47
            "Contul sau parola nu pot fi goale",                    //48
            "Parola nu poate fi goală",                             //49
            "Datele de conectare sunt incorecte, autentificare eșuată", //50
            "Eroare la autentificarea în cloud: {0}",               //51
            "Status: {0}\nDimensiune stocare: {1}\nUltima încărcare: {2}\nNume salvare: {3}\nUltima verificare: {4}", //52
            "Savegame existent",                                    //53
            "Nu s-a găsit niciun savegame",                         //54
            "Eroare la încărcarea informațiilor de salvare: {0}",   //55
            "Încarcă savegame-ul",                                  //56
            "Progres încărcare: {0}%, Dimensiune: {1}",             //57  
            "Progres descărcare: {0}%, Dimensiune: {1}",            //58
            "Savegame încărcat",                                    //59
            "Descarcă savegame-ul",                                 //60
            "Savegame descărcat",                                   //61
            "Extrage savegame-ul",                                  //62
            "Curăță fișierele temporare",                           //63
            "Informații cont PP2",                                  //64
            "Nume cont",                                            //65
            "Parolă cont",                                          //66
            "Autentificare",                                        //67
            "Deconectare",                                          //68
            "Reîmprospătează",                                      //69
            "Încarcă",                                              //70
            "Descarcă",                                             //71
            "Folder backup",                                        //72
            "Folder savegame",                                      //73
            "Număr maxim de salvări",                               //74
            "Curăță",                                               //75
            //Service UI-Langs end
            //Update UI-Langs
            "Pornește verificarea jocului",                         //76
            "Joc despachetat detectat, actualizarea oprită",        //77
            "Nu s-a detectat o instalare normală, actualizarea oprită", //78
            "Fișiere găsite: {0} Foldere: {1}",                     //79
            "Conectare la server",                                  //80
            "Conectare la server: OK, începe verificarea fișierelor", //81
            "Nu s-a putut conecta la server",                       //82
            "Comparare fișiere, faza 1",                            //83
            "Faza 1: verificare fișiere lipsă",                     //84
            "Fișiere lipsă: {0}",                                   //85
            "Lipsește: {0} - {1}",                                  //86
            "Verificare fișiere modificate",                        //87
            "Comparare fișiere, faza 2",                            //88
            "Faza 2: verificare fișier: ",                          //89
            "Fișier: {0} ◄ Necesită actualizare, fișier diferit [{1}|{2}]", //90
            "Fișier: {0} ◄ Este la zi",                             //91
            "Verificarea fișierelor a eșuat. Reîncearcă.",          //92
            "Verificare fișiere finalizată în: {0}",                //93
            "Nu s-a putut obține lista online de fișiere. Reîncearcă mai târziu.", //94
            "Lista de fișiere live descărcată.",                    //95
            "Prea multe fișiere lipsă. Actualizare anulată. Reinstalează jocul.", //96
            "Fișiere și foldere scanate.",                          //97
            "Folder lipsă creat: ",                                 //98
            "Comparare fișiere, faza 3",                            //99
            "Faza 3: verificare integritate fișiere",               //100
            "Faza 3: descărcare și instalare fișiere",              //101
            "Descărcare fișier: {0} ({1}% complet)",                //102
            "Actualizare finalizată în: {0}",                       //103
            "Actualizare joc finalizată.",                          //104
            "Fișier actualizat: ",                                  //105
            "Extracția fișierului a eșuat: ({0}). Eroare: {1}",     //106
            "Fișier: {0} ◄ Instalare în curs",                      //107
            "Verificarea jocului finalizată",                       //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Modul de securitate",             //109
            "Modulul de securitate nu este sigur. Jocul a fost închis.", //110
            "A fost detectată o versiune Steam a jocului. Este corect?", //111
            "A fost detectată o versiune despachetată a jocului. Este corect?", //112
            "Avertisment: folosirea jocurilor despachetate se face pe propria răspundere!\nNu oferim suport pentru acestea!", //113
            "Avertisment: dacă aceasta este o versiune despachetată, unele acțiuni pot deteriora jocul\nsau pot necesita reinstalare!", //114
            "Servere de joc online, jucători: ",                    //115
            "Eroare la obținerea informațiilor. Apasă aici pentru a reîmprospăta", //116
            //Update UI-Langs end
            //Mod Detail View
            "Înapoi",                                               //117
            "Autor",                                                //118
            "Descriere",                                            //119
            "Versiune",                                             //120
            "Fișiere",                                              //121
            "Dimensiune descărcare",                                //122
            "Dimensiune instalare",                                 //123
            "Actualizări",                                          //124
            "Instalări",                                            //125
            "Instalează | Actualizează",                            //126
            "Nume mod",                                             //127
            "Pornește descărcarea modului",                         //128
            "Instalarea modului ({0}) finalizată",                  //129
            "Instalare mod ({0}) fișier: {1} din {2}",              //130
            "Moduri instalate",                                     //131
            "Afișează: {0} moduri din {1}",                         //132
            "Niciun mod disponibil pentru acest tip de joc",        //133
            "Se încarcă modurile",                                  //134
            "Modul: {0} eliminat cu succes",                        //135
            "Modurile despachetate nu pot fi eliminate din launcher.", //136
            "ModId invalid, reîncearcă după repornire",             //137
            "Un mare mulțumesc echipei noastre și susținătorilor – sprijinul și contribuțiile voastre înseamnă enorm pentru noi.\nDe la ajutorul pe Discord până la servere – voi faceți posibil acest proiect. Vă apreciem enorm.", //138
            "Launcher-ul are o versiune veche a bazei de date.\nDorești să convertești setările vechi într-un nou profil?", //139
            "Trebuie mai întâi să creezi un profil de joc\nînainte de a juca.", //140
            "Verifică setările profilului.\nProfil creat cu succes ca: Converted Profile", //141
        };

        public static string[] Turkish =
        {
            //Main UI-Langs
            "Oyna",                                                 //0
            "Başlat",                                               //1
            "Ayarlar",                                              //2
            "Profiller",                                            //3
            "Servis",                                               //4
            "Bilgi",                                                //5
            "Hakkında",                                             //6
            "Çıkış",                                                //7
            "Durumu Kontrol Et",                                    //8
            "Geri",                                                 //9
            "Yapımcı",                                              //10
            "Açıklama",                                              //11
            "Güncellemeler",                                        //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Discord Durumu",                                       //13
            "Güncelleme Kontrolü",                                  //14
            "UPnP Port Yönlendirme",                                //15
            "Dil",                                                  //16
            "Ses Modu",                                              //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "Yeni Profil Oluştur",                                  //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "Yeni Oyun Oturumu Oluştur",                            //19
            "Oyun Oturumunu Düzenle",                               //20
            "Profil Adı",                                           //21
            "Argümanları",                                 //22
            "Dinamik RAM",                                          //23
            "Dinamik Çekirdekler",                                  //24
            "Daha Yüksek Öncelik",                                  //25
            "Otomatik Kir",                                          //26
            "Otomatik Hasar",                                        //27
            "Çevrimiçi Mod",                                        //28
            "Oyun Seç",                                              //29
            "Klasör Aç",                                             //30
            "Bağlantı Yaması",                                       //31
            "Mod Tarayıcı",                                          //32
            "Oyun Güncellemesi",                                     //33
            "Kaydet",                                                //34
            "Oyun Versiyonu: ",                                      //35
            "Oyun Derlemesi: {0}",                                   //36
            "Oyun Yolu: {0}",                                        //37
            "Kurulum Boyutu: {0}",                                   //38
            "Oyun Türü: {0} '{1}' - {2}",                            //39
            " (Yanlış Oyun Versiyonu) Güncelleme gerekli",          //40
            "Test Drive Unlimited oyununu seç",                     //41
            "Yapılandırma dosyası değiştirildi",                     //42
            "Oyun dizini mevcut değil.",                             //43
            "Klasör mevcut değil: ",                                 //44
            "Profil: {0} başarıyla kaydedildi",                      //45
            "Profil kaydedilirken hata: {0} Hata: {1}",             //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "Hesap veya şifre boş olamaz",                          //47
            "Hesap veya şifre boş olamaz",                          //48
            "Şifre boş olamaz",                                      //49
            "Yanlış giriş bilgileri, giriş yapılamıyor",            //50
            "Buluta girişte hata: {0}",                              //51
            "Durum: {0}\nDepolama Boyutu: {1}\nSon Yükleme: {2}\nSavegame Adı: {3}\nSon Kontrol: {4}", //52
            "Savegame mevcut",                                       //53
            "Savegame mevcut değil",                                 //54
            "Savegame bilgileri yüklenirken hata: {0}",              //55
            "Savegame Yükle",                                        //56
            "Yükleme İlerleme: {0}%, Boyut: {1}",                     //57  
            "İndirme İlerleme: {0}%, Boyut: {1}",                     //58
            "Savegame Yüklendi",                                     //59
            "Savegame İndir",                                        //60
            "Savegame İndirildi",                                    //61
            "Savegame Çıkart",                                       //62
            "Geçici Dosyaları Temizle",                               //63
            "PP2 Hesap Bilgileri",                                   //64
            "Hesap Adı",                                             //65
            "Hesap Şifresi",                                         //66
            "Giriş",                                                 //67
            "Çıkış Yap",                                             //68
            "Yenile",                                                //69
            "Yükle",                                                 //70
            "İndir",                                                 //71
            "Yedekleme Klasörü",                                     //72
            "Savegame Klasörü",                                      //73
            "Maks. Kayıt Noktası",                                   //74
            "Temizle",                                               //75
            //Service UI-Langs end
            //Update UI-Langs
            "Oyun kontrolü başlatılıyor",                            //76
            "Paketlenmemiş oyun tespit edildi, güncelleme durduruldu", //77
            "Normal oyun kurulumu tespit edilmedi, güncelleme durduruldu", //78
            "Bulunan Dosyalar: {0} Klasörler: {1}",                  //79
            "Sunucuya bağlanılıyor",                                 //80
            "Sunucuya bağlantı: Tamam, Dosya kontrolü başlatılıyor", //81
            "Sunucuya bağlanılamadı",                                //82
            "Dosyalar karşılaştırılıyor, aşama 1",                   //83
            "Aşama 1: Eksik dosyalar kontrol ediliyor",              //84
            "Eksik dosyalar: {0}",                                   //85
            "Eksik: {0} - {1}",                                      //86
            "Dosyalar değişiklikler için kontrol ediliyor",          //87
            "Dosyalar karşılaştırılıyor, aşama 2",                   //88
            "Aşama 2: Dosya kontrol ediliyor: ",                     //89
            "Dosya: {0} ◄ Güncelleme gerekli, dosya farklı [{1}|{2}]", //90
            "Dosya: {0} ◄ Dosya güncel",                             //91
            "Dosya kontrolü başarısız oldu. Lütfen tekrar deneyin.", //92
            "Dosya kontrolü tamamlandı: {0}",                        //93
            "Çevrimiçi dosya listesi alınamadı. Lütfen daha sonra tekrar deneyin.", //94
            "Canlı dosya listesi indirildi.",                        //95
            "Çok fazla eksik dosya var. Güncelleme iptal edildi. Oyunu yeniden kurun.", //96
            "Dosyalar ve klasörler tarandı.",                        //97
            "Eksik klasör oluşturuldu: ",                            //98
            "Dosyalar karşılaştırılıyor, aşama 3",                   //99
            "Aşama 3: Dosya bütünlüğü kontrol ediliyor",             //100
            "Aşama 3: Dosyalar indiriliyor ve kuruluyor",            //101
            "Dosya indiriliyor: {0} (%{1} tamamlandı)",              //102
            "Güncelleme tamamlandı: {0}",                            //103
            "Oyun güncellemesi tamamlandı.",                         //104
            "Dosya güncellendi: ",                                   //105
            "Dosya çıkarma başarısız oldu: ({0}). Hata: {1}",        //106
            "Dosya: {0} ◄ Kuruluyor",                               //107
            "Oyun kontrolü tamamlandı",                              //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - Güvenlik Modülü",                  //109
            "Güvenlik modülü güvenli değil. Oyun kapatıldı.",        //110
            "Steam oyunu tespit edildi, doğru mu?",                  //111
            "Paketlenmemiş oyun tespit edildi, doğru mu?",           //112
            "Uyarı: Paketlenmemiş oyunları kullanmak risklidir!\nHerhangi bir yardım veya destek sunmuyoruz!", //113
            "Uyarı: Bu gerçekten paketlenmemiş bir oyun ise, bazı işlemler oyunu bozabilir\ndolayısıyla yeniden kurulum gerekebilir!", //114
            "Oyun sunucuları çevrimiçi, oyuncu sayısı: ",           //115
            "Bilgi alınamadı. Yenilemek için tıklayın",             //116
            //Update UI-Langs end
            //Mod Detail View
            "Geri",                                                 //117
            "Yapımcı",                                              //118
            "Açıklama",                                              //119
            "Versiyon",                                              //120
            "Dosyalar",                                              //121
            "İndirme Boyutu",                                        //122
            "Kurulum Boyutu",                                        //123
            "Güncellemeler",                                         //124
            "Kurulumlar",                                            //125
            "Kur | Güncelle",                                        //126
            "Mod Adı",                                               //127
            "Mod İndirme Başlatılıyor",                              //128
            "Mod Kurulumu ({0}) Tamamlandı",                         //129
            "Mod ({0}) kuruluyor dosya: {1} / {2}",                  //130
            "Kurulu Modlar",                                         //131
            "Gösteriliyor: {0} moddan {1}",                          //132
            "Bu oyun türü için mod mevcut değil",                    //133
            "Modlar yükleniyor",                                     //134
            "Mod: {0} başarıyla kaldırıldı",                         //135
            "Paketlenmemiş modlar launcher üzerinden kaldırılamaz.", //136
            "Geçersiz ModId, yeniden başlattıktan sonra tekrar deneyin", //137
            "Özel teşekkürler \nHarika ekibimize ve destekçilerimize – desteğiniz, katkılarınız bizim için çok değerli. \nDiscord’daki yardımdan sunucuların çalıştırılmasına kadar – bu projeyi siz mümkün kılıyorsunuz. Hepinizi çok takdir ediyoruz.", //138
            "Launcher eski bir veri tabanı sürümüne sahip.\nEski ayarları yeni bir profile dönüştürmek ister misiniz?", //139
            "Oynamadan önce oyun profili oluşturmanız gerekir.",   //140
            "Lütfen profil ayarlarını kontrol edin.\nProfil başarıyla oluşturuldu: Converted Profile", //141
        };

        public static string[] Chinese =
        {
            //Main UI-Langs
            "开始游戏",                                              //0
            "开始",                                                  //1
            "设置",                                                  //2
            "配置文件",                                              //3
            "服务",                                                  //4
            "信息",                                                  //5
            "关于",                                                  //6
            "退出",                                                  //7
            "检查状态",                                              //8
            "返回",                                                  //9
            "作者",                                                  //10
            "描述",                                                  //11
            "更新",                                                  //12
            //Main UI-Langs end
            //Settings UI-Langs
            "Discord状态",                                          //13
            "检查更新",                                              //14
            "UPnP端口转发",                                          //15
            "语言",                                                  //16
            "音效模式",                                              //17
            //Settings UI-Langs end
            //Profile UI-Langs
            "创建新配置文件",                                        //18
            //Profile UI-Langs end
            //Create Profile UI-Langs
            "创建新的游戏实例",                                      //19
            "编辑游戏实例",                                          //20
            "配置文件名",                                            //21
            "启动参数",                                              //22
            "动态内存",                                              //23
            "动态核心",                                              //24
            "更高优先级",                                            //25
            "自动污损",                                              //26
            "自动损伤",                                              //27
            "在线模式",                                              //28
            "选择游戏",                                              //29
            "打开文件夹",                                            //30
            "连接补丁",                                              //31
            "模组浏览器",                                            //32
            "游戏更新",                                              //33
            "保存",                                                  //34
            "游戏版本: ",                                            //35
            "游戏构建: {0}",                                         //36
            "游戏路径: {0}",                                         //37
            "游戏安装大小: {0}",                                      //38
            "游戏类型: {0} '{1}' - {2}",                             //39
            "（游戏版本错误）需要更新",                              //40
            "选择你的Test Drive Unlimited游戏",                     //41
            "配置文件已替换",                                        //42
            "游戏目录不存在",                                        //43
            "文件夹不存在: ",                                        //44
            "配置文件: {0} 保存成功",                                 //45
            "保存配置文件时出错: {0} 错误: {1}",                     //46
            //Create Profile UI-Langs end
            //Service UI-Langs 
            "账号或密码不能为空",                                    //47
            "账号或密码不能为空",                                    //48
            "密码不能为空",                                          //49
            "登录信息错误，无法登录",                                //50
            "云端登录出错: {0}",                                     //51
            "状态: {0}\n存储大小: {1}\n最后上传: {2}\n存档名: {3}\n最后检查: {4}", //52
            "存档存在",                                              //53
            "没有存档",                                              //54
            "加载存档信息出错: {0}",                                  //55
            "上传存档",                                              //56
            "上传进度: {0}%, 大小: {1}",                              //57  
            "下载进度: {0}%, 大小: {1}",                              //58
            "存档已上传",                                            //59
            "下载存档",                                              //60
            "存档已下载",                                            //61
            "解压存档",                                              //62
            "临时文件清理",                                          //63
            "PP2账号信息",                                           //64
            "账号名",                                                //65
            "账号密码",                                              //66
            "登录",                                                  //67
            "登出",                                                  //68
            "刷新",                                                  //69
            "上传",                                                  //70
            "下载",                                                  //71
            "备份文件夹",                                            //72
            "存档文件夹",                                            //73
            "最大存档点数",                                          //74
            "清理",                                                  //75
            //Service UI-Langs end
            //Update UI-Langs
            "开始游戏检测",                                          //76
            "检测到解包游戏，停止更新",                               //77
            "未检测到正常游戏安装，停止更新",                         //78
            "找到文件: {0} 文件夹: {1}",                             //79
            "连接服务器",                                            //80
            "连接服务器成功，开始文件检查",                           //81
            "无法连接服务器",                                        //82
            "比较文件，第1阶段",                                      //83
            "第1阶段: 检查缺失文件",                                  //84
            "缺失文件: {0}",                                          //85
            "缺少: {0} - {1}",                                       //86
            "检查文件更改",                                           //87
            "比较文件，第2阶段",                                      //88
            "第2阶段: 检查文件: ",                                    //89
            "文件: {0} ◄ 需要更新，文件不同 [{1}|{2}]",              //90
            "文件: {0} ◄ 已是最新",                                   //91
            "文件检查失败，请重试。",                                 //92
            "文件检查完成，耗时: {0}",                                //93
            "无法获取在线文件列表，请稍后重试。",                      //94
            "在线文件列表已下载。",                                   //95
            "缺少文件过多，更新中止，请重新安装游戏。",               //96
            "文件和文件夹已扫描。",                                   //97
            "缺失目录已创建: ",                                       //98
            "比较文件，第3阶段",                                      //99
            "第3阶段: 验证文件完整性",                                 //100
            "第3阶段: 下载并安装文件",                                 //101
            "正在下载文件: {0} ({1}% 完成)",                          //102
            "更新完成，耗时: {0}",                                     //103
            "游戏更新完成。",                                         //104
            "文件已更新: ",                                           //105
            "解压文件失败: ({0}) 错误: {1}",                          //106
            "文件: {0} ◄ 正在安装",                                   //107
            "游戏检测完成",                                           //108
            //Gameupdate end
            //Generic Messages
            "Project Paradise 2 - 安全模块",                          //109
            "安全模块不安全，游戏已关闭。",                             //110
            "检测到Steam游戏，是否正确？",                            //111
            "检测到解包游戏，是否正确？",                               //112
            "警告：使用解包游戏风险自负！\n我们不提供相关帮助或支持！",     //113
            "警告：如果这是解包游戏，某些操作可能会损坏游戏\n并导致重新安装！", //114
            "游戏服务器在线，玩家数: ",                              //115
            "获取信息失败，点击这里刷新",                              //116
            //Update UI-Langs end
            //Mod Detail View
            "返回",                                                  //117
            "作者",                                                  //118
            "描述",                                                  //119
            "版本",                                                  //120
            "文件",                                                  //121
            "下载大小",                                              //122
            "安装大小",                                              //123
            "更新",                                                  //124
            "安装记录",                                              //125
            "安装 | 更新",                                           //126
            "模组名称",                                              //127
            "开始模组下载",                                           //128
            "模组安装完成 ({0})",                                     //129
            "正在安装模组 ({0}) 文件: {1} / {2}",                     //130
            "已安装模组",                                             //131
            "显示: {0} 个模组，共 {1}",                               //132
            "该游戏类型没有可用模组",                                 //133
            "加载模组",                                               //134
            "模组: {0} 已成功卸载",                                   //135
            "解包模组无法通过启动器卸载。",                           //136
            "错误的ModId，请重启后再试",                              //137
            "特别感谢我们的团队和赞助者——您的支持、投入和贡献对我们意义重大。\n从Discord的帮助到服务器运行，都是您让这个项目成为可能。我们非常感激每一个人。", //138
            "启动器使用旧数据库版本。\n是否将旧设置转换为新配置文件？", //139
            "在开始游戏之前必须创建游戏配置文件。",                 //140
            "请检查配置文件设置。\n配置文件已成功创建: Converted Profile", //141
        };
    }
}