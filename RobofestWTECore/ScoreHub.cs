﻿using System;
using System.Linq;
using RobofestWTECore.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RobofestWTECore.Models;
using RobofestWTE.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using RobofestWTECore.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace RobofestWTECore
{
    public class ScoreHub : Hub
    {
        private readonly GameContext db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext context;
        private readonly IHubContext<ScoreHub> _hubContext;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private static Dictionary<int, StaticField> Fields = new Dictionary<int, StaticField>
            {
                { 1, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 2, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 3, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 4, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 5, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 6, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
            };
        private static Dictionary<int, StaticField> OnDeck = new Dictionary<int, StaticField>
            {
                { 1, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 2, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 3, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 4, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 5, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
                { 6, new StaticField { TeamID = 0, TeamNumber = "EMPTY", CurrentScore = 0, Data = "", Status = 0, HelpNeeded = false} },
            };
        private static int ScorekeeperStatus;
        private static bool JudgesLocked = true;
        private static Dictionary<int, StaticCompetition> RunningComp = new Dictionary<int, StaticCompetition>();
        private static string appSessionID = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        private static Dictionary<string, RoAuthUser> appAuthUsers = new Dictionary<string, RoAuthUser>();
        private static Dictionary<int, List<String>> compGroups = new Dictionary<int, List<string>>();
        //0 = BUSY, 1 = REVIEWING, 2 = OK


        public ScoreHub(ApplicationDbContext context, GameContext db, IHubContext<ScoreHub> hubContext, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager2, RoleManager<IdentityRole> roleManager2, SignInManager<ApplicationUser> signInManager)
        {
            this.context = context;
            this.db = db;
            _signInManager = signInManager;
            _hubContext = hubContext;
            userManager = userManager2;
            roleManager = roleManager2;
        }
        public async Task InitializeClient(int CompID)
        {
            if (compGroups.ContainsKey(CompID))
            {
                compGroups[CompID].Add(Context.ConnectionId);
            }
            else
            {
                await RegisterCompetitionAsActive(CompID);
                compGroups.Add(CompID, new List<String>());
                compGroups[CompID].Add(Context.ConnectionId);
            }
        }
        public async Task RegisterCompetitionAsActive(int CompID)
        {
            var newCompetition = new StaticCompetition();
            if (!RunningComp.ContainsKey(CompID))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Comp" + CompID.ToString());
                RunningComp.Add(CompID, newCompetition);
                //FIND COMP ROLE
                if(!roleManager.RoleExistsAsync(CompID.ToString() + "-Judge").Result)
                {
                    //START CREATING COMP ROLES
                    var judgerole = new IdentityRole();
                    judgerole.Name = CompID.ToString() + "-Judge";
                    judgerole.NormalizedName = CompID.ToString() + "-JUDGE";
                    judgerole.Id = Guid.NewGuid().ToString();
                    await roleManager.CreateAsync(judgerole);
                    var fieldrole = new IdentityRole();
                    fieldrole.Name = CompID.ToString() + "-Field";
                    fieldrole.NormalizedName = CompID.ToString() + "-FIELD";
                    fieldrole.Id = Guid.NewGuid().ToString();
                    await roleManager.CreateAsync(fieldrole);
                    var techrole = new IdentityRole();
                    techrole.Name = CompID.ToString() + "-Tech";
                    techrole.NormalizedName = CompID.ToString() + "-TECH";
                    techrole.Id = Guid.NewGuid().ToString();
                    await roleManager.CreateAsync(techrole);
                    var managerrole = new IdentityRole();
                    managerrole.Name = CompID.ToString() + "-Manager";
                    managerrole.NormalizedName = CompID.ToString() + "-MANAGER";
                    managerrole.Id = Guid.NewGuid().ToString();
                    await roleManager.CreateAsync(managerrole);
                    var mainrole = new IdentityRole();
                    mainrole.Name = CompID.ToString() + "-Main";
                    mainrole.NormalizedName = CompID.ToString() + "-MAIN";
                    mainrole.Id = Guid.NewGuid().ToString();
                    await roleManager.CreateAsync(mainrole);
                    var adminrole = new IdentityRole();
                    adminrole.Name = CompID.ToString() + "-Admin";
                    adminrole.NormalizedName = CompID.ToString() + "-ADMIN";
                    adminrole.Id = Guid.NewGuid().ToString();
                    await roleManager.CreateAsync(adminrole);
                }
                if (!roleManager.RoleExistsAsync("Comp" + CompID.ToString()).Result)
                {
                    //START CREATING COMP ROLE
                    var comprole = new IdentityRole();
                    comprole.Name = "Comp" + CompID.ToString();
                    comprole.NormalizedName = "COMP" + CompID.ToString();
                    comprole.Id = (900000+CompID).ToString();
                    await roleManager.CreateAsync(comprole);
                    
                }
            }
            
        }
        public async Task SetupMatch(string Password, string UserName, int compid, int judges, int fieldstaff, int managers, int tech)
        {
            var CompetitionID = (from c in db.Competitions where c.CompID == compid select c).FirstOrDefault();
            if(CompetitionID.setup == false)
            {
                if (Password == "RobofestWTE")
                {
                        
                    /*IdentityRole judge = new IdentityRole();
                    judge.Name = "Judge";
                    judge.NormalizedName = "JUDGE";
                    judge.Id = "4";
                    IdentityRole field1a = new IdentityRole();
                    field1a.Name = "Field1";
                    field1a.NormalizedName = "FIELD1";
                    field1a.Id = "1001";
                    IdentityRole field2a = new IdentityRole();
                    field2a.Name = "Field2";
                    field2a.NormalizedName = "FIELD2";
                    field2a.Id = "1002";
                    IdentityRole field3a = new IdentityRole();
                    field3a.Name = "Field3";
                    field3a.NormalizedName = "FIELD3";
                    field3a.Id = "1003";
                    IdentityRole field4a = new IdentityRole();
                    field4a.Name = "Field4";
                    field4a.NormalizedName = "FIELD4";
                    field4a.Id = "1004";
                    IdentityRole field5a = new IdentityRole();
                    field5a.Name = "Field5";
                    field5a.NormalizedName = "FIELD5";
                    field5a.Id = "1005";
                    IdentityRole field6a = new IdentityRole();
                    field6a.Name = "Field6";
                    field6a.NormalizedName = "FIELD6";
                    field6a.Id = "1006";
                    IdentityRole allfields = new IdentityRole();
                    field6a.Name = "AllFields";
                    field6a.NormalizedName = "ALLFIELDS";
                    field6a.Id = "1000";
                    IdentityRole admin = new IdentityRole();
                    admin.Name = "Admin";
                    admin.NormalizedName = "ADMIN";
                    admin.Id = "1";
                    IdentityRole techrole = new IdentityRole();
                    techrole.Name = "Tech";
                    techrole.NormalizedName = "TECH";
                    techrole.Id = "-1";
                    IdentityRole managerrole = new IdentityRole();
                    managerrole.Name = "Manager";
                    managerrole.NormalizedName = "MANAGER";
                    managerrole.Id = "2";
                    IdentityRole fieldstaffrole = new IdentityRole();
                    fieldstaffrole.Name = "FieldStaff";
                    fieldstaffrole.NormalizedName = "FIELDSTAFF";
                    fieldstaffrole.Id = "3";
                    IdentityRole locked = new IdentityRole();
                    locked.Name = "Locked";
                    locked.NormalizedName = "LOCKED";
                    locked.Id = "5";
                    await Clients.All.SendAsync("setupProgress", 5, "Roles Created, Uploading...");
                    await roleManager.CreateAsync(judge);
                    Console.WriteLine("Judge Created");
                    await roleManager.CreateAsync(field1a);
                    await roleManager.CreateAsync(field2a);
                    await roleManager.CreateAsync(field3a);
                    await roleManager.CreateAsync(field4a);
                    await roleManager.CreateAsync(field5a);
                    await roleManager.CreateAsync(field6a);
                    await roleManager.CreateAsync(allfields);
                    await roleManager.CreateAsync(admin);
                    await roleManager.CreateAsync(techrole);
                    await roleManager.CreateAsync(managerrole);
                    await roleManager.CreateAsync(fieldstaffrole);
                    await roleManager.CreateAsync(locked);*/
                    
                    await Clients.All.SendAsync("setupProgress", 10, "Roles Uploaded to Database!");
                    System.Threading.Thread.Sleep(5000);
                    
                    await userManager.AddToRoleAsync(context.Users.Where(u => u.UserName == UserName).FirstOrDefault(), "Main");
                    
                    
                    await Clients.All.SendAsync("setupProgress", 20, "Competition Status & User Account Updated!");
                    for (int j = 0;j < judges; j++)
                    {
                        ApplicationUser JudgeToAdd = new ApplicationUser();
                        JudgeToAdd.UserName = "Judge" + (j + 1).ToString();
                        JudgeToAdd.NormalizedUserName = "JUDGE" + (j + 1).ToString();
                        JudgeToAdd.Email = JudgeToAdd.UserName + "@robofest.com";
                        JudgeToAdd.NormalizedEmail = JudgeToAdd.NormalizedUserName = "@ROBOFEST.COM";
                        JudgeToAdd.Id = Guid.NewGuid().ToString();
                        string PublicPass = "J!"+Guid.NewGuid().ToString("n").Substring(0, 8);
                        await userManager.CreateAsync(JudgeToAdd, PublicPass);
                        Console.WriteLine("Test Success");
                        ApplicationUser UserActual = await userManager.FindByEmailAsync(JudgeToAdd.Email);
                        await userManager.AddToRoleAsync(UserActual, "JUDGE");
                        await userManager.AddToRoleAsync(UserActual, "ALLFIELDS");
                        PresetAccount presetAccount = new PresetAccount();
                        presetAccount.CompID = compid;
                        presetAccount.Email = JudgeToAdd.Email;
                        presetAccount.PresetAccoutID = UserActual.Id;
                        presetAccount.PublicPasskey = PublicPass;
                        presetAccount.Username = JudgeToAdd.UserName;
                        await db.PresetAccounts.AddAsync(presetAccount); 
                        await db.SaveChangesAsync();
                    }
                    await Clients.All.SendAsync("setupProgress", 40, "Judge Roles Uploaded to Database!");
                    for (int j = 0; j < fieldstaff; j++)
                    {
                        ApplicationUser FSToAdd = new ApplicationUser();
                        FSToAdd.UserName = "FieldStaff" + (j + 1).ToString();
                        FSToAdd.NormalizedUserName = "FIELDSTAFF" + (j + 1).ToString();
                        //JudgeToAdd.Id = Guid.NewGuid().ToString();
                        FSToAdd.Email = FSToAdd.UserName + "@robofest.com";
                        FSToAdd.NormalizedEmail = FSToAdd.NormalizedUserName = "@ROBOFEST.COM";
                        FSToAdd.Id = Guid.NewGuid().ToString();
                        string PublicPass = "FS!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                        
                        await userManager.CreateAsync(FSToAdd, PublicPass);
                        ApplicationUser UserActual = await userManager.FindByEmailAsync(FSToAdd.Email);
                        await userManager.AddToRoleAsync(UserActual, "FIELDSTAFF");
                        PresetAccount presetAccount = new PresetAccount();
                        presetAccount.CompID = compid;
                        presetAccount.Email = FSToAdd.Email;
                        presetAccount.PresetAccoutID = UserActual.Id;
                        presetAccount.PublicPasskey = PublicPass;
                        presetAccount.Username = FSToAdd.UserName;
                        await db.PresetAccounts.AddAsync(presetAccount);
                        await db.SaveChangesAsync();
                    }
                    await Clients.All.SendAsync("setupProgress", 60, "Field Staff Roles Uploaded to Database!");
                    for (int j = 0; j < managers; j++)
                    {
                        ApplicationUser ManagerToAdd = new ApplicationUser();
                        ManagerToAdd.UserName = "Manager" + (j + 1).ToString();
                        ManagerToAdd.NormalizedUserName = "MANAGER" + (j + 1).ToString();
                        //JudgeToAdd.Id = Guid.NewGuid().ToString();
                        ManagerToAdd.Email = ManagerToAdd.UserName + "@robofest.com";
                        ManagerToAdd.NormalizedEmail = ManagerToAdd.NormalizedUserName = "@ROBOFEST.COM";
                        ManagerToAdd.Id = Guid.NewGuid().ToString();
                        string PublicPass = "M!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                        
                        await userManager.CreateAsync(ManagerToAdd, PublicPass);
                        ApplicationUser UserActual = await userManager.FindByEmailAsync(ManagerToAdd.Email);
                        await userManager.AddToRoleAsync(UserActual, "MANAGER");
                        PresetAccount presetAccount = new PresetAccount();
                        presetAccount.CompID = compid;
                        presetAccount.Email = ManagerToAdd.Email;
                        presetAccount.PresetAccoutID = UserActual.Id;
                        presetAccount.PublicPasskey = PublicPass;
                        presetAccount.Username = ManagerToAdd.UserName;
                        await db.PresetAccounts.AddAsync(presetAccount);
                        await db.SaveChangesAsync();
                    }
                    await Clients.All.SendAsync("setupProgress", 80, "Manager Roles Uploaded to Database!");
                    for (int j = 0; j < tech; j++)
                    {
                        ApplicationUser TechToAdd = new ApplicationUser();
                        TechToAdd.UserName = "Tech" + (j + 1).ToString();
                        TechToAdd.NormalizedUserName = "TECH" + (j + 1).ToString();
                        //JudgeToAdd.Id = Guid.NewGuid().ToString();
                        TechToAdd.Email = TechToAdd.UserName + "@robofest.com";
                        TechToAdd.NormalizedEmail = TechToAdd.NormalizedUserName = "@ROBOFEST.COM";
                        TechToAdd.Id = Guid.NewGuid().ToString();
                        string PublicPass = "T!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                        
                        await userManager.CreateAsync(TechToAdd, PublicPass);
                        ApplicationUser UserActual = await userManager.FindByEmailAsync(TechToAdd.Email);
                        await userManager.AddToRoleAsync(UserActual, "TECH");
                        PresetAccount presetAccount = new PresetAccount();
                        presetAccount.CompID = compid;
                        presetAccount.Email = TechToAdd.Email;
                        presetAccount.PresetAccoutID = UserActual.Id;
                        presetAccount.PublicPasskey = PublicPass;
                        presetAccount.Username = TechToAdd.UserName;
                        await db.PresetAccounts.AddAsync(presetAccount);
                        await db.SaveChangesAsync();
                    }
                    await Clients.All.SendAsync("setupProgress", 99, "Tech Roles Uploaded to Database!");
                    await Clients.All.SendAsync("setupProgress", 100, "RSO Setup Complete!");
                    await Clients.All.SendAsync("setupSuccess");
                    CompetitionID.setup = true;
                    db.Competitions.Update(CompetitionID);
                    await db.SaveChangesAsync();
                }
                else
                {
                    await Clients.All.SendAsync("setupFailure");
                }
            }
            else
            {
                await Clients.All.SendAsync("setupFailure");
            }
            
            
        }
        public void SendTimer(int minutes, int seconds, string message, int status, int compid)
        {
            Clients.Clients(compGroups[compid]).SendAsync("changeGlobalTimer", minutes, seconds, message, status);
            Clients.All.SendAsync("changeGlobalTimerWPF", minutes, seconds, message, status);
            //0 = STOPPED, 1 = RUNNING, 2 = DONE
        }
        public void StartTimer()
        {
            Clients.All.SendAsync("startGlobalTimer");
        }
        public async Task ChangeMatches(string jsonrequired, int fields, int compid)
        {
            //PKs need to be generated from a GUID, not from a consecutive number
            //ORDER should be the sorting method for all entries from now on.
            var CompetitionFields = (from d in db.Competitions where d.CompID == compid select d).FirstOrDefault();
            CompetitionFields.RunningFields = fields;
            db.Competitions.Update(CompetitionFields);
            TeamMatch Match = JsonConvert.DeserializeObject<TeamMatch>(jsonrequired);
            await db.TeamMatches.AddAsync(Match);
            await db.SaveChangesAsync();
            var amountofmatches = db.TeamMatches.Where(t => t.CompID == compid).Count();
            float matchcountfloat = (float)amountofmatches / (float)CompetitionFields.RunningFields;
            int matchcount = (int)Math.Ceiling(matchcountfloat);
            await Clients.Clients(compGroups[compid]).SendAsync("addMatches", matchcount);
            await Clients.Clients(compGroups[compid]).SendAsync("reloadRequired");
        }
        public async Task SendAllMatches(int fields, int compid)
        {
            var teammatchlist = new List<TeamMatchCondensed>();
            var rawdblist = db.TeamMatches.Where(x => x.CompID == compid).OrderBy(x => x.MatchID).ThenBy(x => x.Order).ToList();
            foreach(var item in rawdblist)
            {
                var newteammatch = new TeamMatchCondensed();
                newteammatch.Test = item.Test;
                newteammatch.TeamNumber = item.TeamNumber;
                newteammatch.Rerun = item.Rerun;
                newteammatch.Completed = item.Completed;
                newteammatch.RoundNum = item.RoundNum;
                
                if(newteammatch.TeamNumber != "EMPTY")
                {
                    var splitTeamNum = newteammatch.TeamNumber.Split("-");
                    var lookUpTeam = db.StudentTeams.Where(x => x.TeamNumberBranch.ToString() == splitTeamNum[0] && x.TeamNumberSpecific.ToString() == splitTeamNum[1]).FirstOrDefault();
                    if(lookUpTeam != null)
                    {
                        newteammatch.Validate = 2;
                    }
                    else
                    {
                        newteammatch.Validate = 1;
                    }
                }
                else
                {
                    newteammatch.Validate = 0;
                }
                teammatchlist.Add(newteammatch);
            }
            await Clients.Clients(compGroups[compid]).SendAsync("allMatches", fields, teammatchlist);
        }
        public async Task ClearSchedule(int compid)
        {
            var EverythingToClear = (from m in db.TeamMatches where m.CompID == compid select m).ToList();
            foreach (var match in EverythingToClear)
            {
                db.TeamMatches.Remove(match);
                await db.SaveChangesAsync();
            }

        }
        public async Task SelectThisMatch(int matchtoselect, int compid)
        {
            var MatchList = (from m in db.TeamMatches where m.CompID == compid select m).OrderBy(m => m.MatchID).ThenBy(m => m.Order).ToList();
            var Competition = (from m in db.Competitions where m.CompID == compid select m).FirstOrDefault();
            var SendBackList = new List<TeamMatch>();
            var skipthismany = (matchtoselect - 1) * Competition.RunningFields;
            foreach(var match in MatchList.Skip(skipthismany).Take(Competition.RunningFields))
            {
                SendBackList.Add(match);
            }
            await Clients.Clients(compGroups[compid]).SendAsync("theseTeams", SendBackList);
        }
        public async Task CheckIfValid(int match, int compid)
        {
            // Is this needed?
            var Competition = (from c in db.Competitions where c.CompID == 1 select c).FirstOrDefault();
            var MatchList = (from m in db.TeamMatches where m.CompID == 1 select m).OrderBy(m => m.MatchID).ThenBy(m => m.Order).ToList();
            var ListToCheck = new List<TeamMatch>();
            var skipthismany = (match - 1) * Competition.RunningFields;
            int[] MatchCheck = { 0, 0, 0, 0, 0, 0 };
            foreach (var matchsort in MatchList.Skip(skipthismany).Take(Competition.RunningFields))
            {
                ListToCheck.Add(matchsort);
            }
            int i = 0;
            foreach(var selectmatch in ListToCheck)
            {
                if (selectmatch.TeamNumber.Contains("-"))
                {
                    var TeamNumberSplit = selectmatch.TeamNumber.Split("-");
                    var CheckToDB = db.StudentTeams.Where(t => t.TeamNumberBranch.ToString() == TeamNumberSplit[0] && t.TeamNumberSpecific.ToString() == TeamNumberSplit[1] && t.CompID == 1).FirstOrDefault();
                    if(CheckToDB != null)
                    {
                        MatchCheck[i] = 1;
                    }
                }
                i++;
            }
            await Clients.Clients(compGroups[compid]).SendAsync("validate", MatchCheck[0], MatchCheck[1], MatchCheck[2], MatchCheck[3], MatchCheck[4], MatchCheck[5]);
        }
        public async Task EditSpecificSchedule(int matchid, string calltype, int compid)
        {
            var SpecificMatch = (from m in db.TeamMatches where m.MatchID == matchid select m).FirstOrDefault();
            if (calltype == "completed")
            {
                if (SpecificMatch.Completed == true)
                {
                    SpecificMatch.Completed = false;
                }
                else
                {
                    SpecificMatch.Completed = true;
                }
            }
            else if (calltype == "testmatch")
            {
                if (SpecificMatch.Test == true)
                {
                    SpecificMatch.Test = false;
                }
                else
                {
                    SpecificMatch.Test = true;
                }
            }
            else if (calltype == "rerun")
            {
                if (SpecificMatch.Rerun == true)
                {
                    SpecificMatch.Rerun = false;
                }
                else
                {
                    SpecificMatch.Rerun = true;
                }
            }
            db.TeamMatches.Update(SpecificMatch);
            await db.SaveChangesAsync();
            await Clients.Clients(compGroups[compid]).SendAsync("reloadRequired");
        }
        public async Task CompleteAll(bool completed, int compid)
        {
            var AllMatches = (from m in db.TeamMatches where m.CompID == compid select m).ToList();
            foreach(var match in AllMatches)
            {
                if(completed == true)
                {
                    match.Completed = true;
                }
                else
                {
                    match.Completed = false;
                }
                db.TeamMatches.Update(match);
                await db.SaveChangesAsync();
            }
            await Clients.Clients(compGroups[compid]).SendAsync("reloadRequired");
        }
        public async Task AutoComplete(int compid)
        {
            var AllMatches = (from m in db.TeamMatches where m.CompID == compid select m).ToList();
            foreach(var match in AllMatches)
            {
                if (match.TeamNumber.Contains("-") != true)
                {
                    continue;
                }
                var TeamNumberParsed = match.TeamNumber.Split("-");
                var TeamID = (from t in db.StudentTeams where t.TeamNumberBranch.ToString() == TeamNumberParsed[0] & t.TeamNumberSpecific.ToString() == TeamNumberParsed[1] select t).First().TeamID;
                if(TeamID == 0)
                {
                    continue;
                }
                if(match.RoundNum == 1)
                {
                    var Round1 = (from r in db.RoundEntries where r.TeamID == TeamID & r.Round == 1 select r).FirstOrDefault();
                    if (Round1 != null)
                    {
                        if(Round1.TeamID == TeamID || Round1.Round == match.RoundNum)
                        {
                            match.Completed = true;
                        }
                        
                    }
                }
                else if (match.RoundNum == 2)
                {
                    var Round2 = (from r in db.RoundEntries where r.TeamID == TeamID & r.Round == 2 select r).First();
                    if (Round2 != null)
                    {
                        if (Round2.TeamID != TeamID || Round2.Round != match.RoundNum)
                        {
                            match.Completed = true;
                        }
                    }
                }
                db.TeamMatches.Update(match);
                await db.SaveChangesAsync();
            }
            await Clients.Clients(compGroups[compid]).SendAsync("reloadRequired");
        }
        public void StopTimer()
        {
            Clients.All.SendAsync("stopGlobalTimer");
        }
        public void InitField(int field, int status, int score, string teamnumber, bool connection, bool matchkeeper, string data, int compid)
        {
            if (RunningComp.ContainsKey(compid))
            {
                Clients.Clients(compGroups[compid]).SendAsync("initFieldView", field, status, score, teamnumber, connection, matchkeeper, data);
                if (matchkeeper != true)
                {

                    RunningComp[compid].Fields[field].ClientConnectionID = Context.ConnectionId;
                    RunningComp[compid].Fields[field].CurrentScore = score;
                    RunningComp[compid].Fields[field].TeamNumber = teamnumber;
                    RunningComp[compid].Fields[field].Data = data;
                }
                RunningComp[compid].Fields[field].Status = status;
                Clients.All.SendAsync("appLiveScores", field, status, score, teamnumber, connection, matchkeeper, data, compid);
            }
            
            
            //0 = NotInit, 1 = NotReady, 2 = Ready
        }
        public void LookUpTeam(int id, int compid)
        {

            var teamnumber = (from t in db.StudentTeams where t.TeamID == id select t).FirstOrDefault().TeamNumberBranch + "-" + (from t in db.StudentTeams where t.TeamID == id select t).FirstOrDefault().TeamNumberSpecific;
            if (teamnumber == null)
            {
                teamnumber = "No Team";
            }
            Clients.Clients(compGroups[compid]).SendAsync("retrieveTeam", teamnumber);
        }
        public void PingField(int field, int compid)
        {
            Clients.Clients(compGroups[compid]).SendAsync("getPong",field);
        }
        public void Pong(int fieldid, string account, int compid)
        {
            Clients.Clients(compGroups[compid]).SendAsync("updateLive", fieldid, account);
        }
        public void SetStage(int stage, int compid)
        {
            Clients.Clients(compGroups[compid]).SendAsync("changeStage", stage);
        }
        public void JudgeHelp(int field, bool helpme, int compid)
        {
            Clients.Clients(compGroups[compid]).SendAsync("helpThisJudge", field, helpme);
        }
        public void ScoreCheck(int field, string data, int score, int roundid, string teamnumber, int compid)
        {
            // Is this needed?
            Clients.Clients(compGroups[compid]).SendAsync("checkThisScore", field, data, score, roundid, teamnumber);
        }
        public void SendBroadcast(string message, bool issue, string sender, bool volunteersonly, int compid)
        {
            Clients.Clients(compGroups[compid]).SendAsync("broadcast", message, issue, sender, volunteersonly);
            Clients.Clients(compGroups[compid]).SendAsync("chatMessage",message, issue, sender, DateTime.Now, true);
        }
        public void SendMessage(string message, bool issue, string sender, int compid)
        {
            Clients.Clients(compGroups[compid]).SendAsync("chatMessage",message, issue, sender, DateTime.Now, false);
        }
        public void TeamSelected(int TeamID, int field, int round, int compid)
        {
            var studentTeam = (from r in db.StudentTeams where r.TeamID == TeamID select r).FirstOrDefault();
            if (round == 1)
            {
                studentTeam.FieldR1 = field;
            }
            else if (round == 2)
            {
                studentTeam.FieldR2 = field;
            }
            db.StudentTeams.Update(studentTeam);
            db.SaveChanges();
            var MatchDataModelSent = new MatchDataModel();
            foreach (var s in db.StudentTeams.ToList())
            {
                var StudentTeam = new StudentTeam();
                var TeamData = db.StudentTeams.Find(s.TeamID);
                StudentTeam.TeamName = TeamData.TeamName;
                StudentTeam.TeamNumberBranch = TeamData.TeamNumberBranch;
                StudentTeam.TeamNumberSpecific = TeamData.TeamNumberSpecific;
                StudentTeam.TeamID = TeamData.TeamID;
                StudentTeam.FieldR1 = TeamData.FieldR1;
                StudentTeam.FieldR2 = TeamData.FieldR2;
                MatchDataModelSent.R1List.Add(StudentTeam);
                MatchDataModelSent.R2List.Add(StudentTeam);
            }
            MatchDataModelSent.R1List = MatchDataModelSent.R1List.Where(a => a.FieldR1 == 0).OrderBy(a => a.TeamNumberBranch).ThenBy(a => a.TeamNumberSpecific).ToList();
            MatchDataModelSent.R2List = MatchDataModelSent.R2List.Where(a => a.FieldR2 == 0).OrderByDescending(a => a.TeamNumberBranch).ThenByDescending(a => a.TeamNumberSpecific).ToList();
            Clients.Clients(compGroups[compid]).SendAsync("changeList", MatchDataModelSent);
        }
        public void MatchMaker(string json, int compid)
        {
            //FIX
            if (RunningComp.ContainsKey(compid))
            {
                var matcheslist = JsonConvert.DeserializeObject<List<ScheduleItem>>(json);
                matcheslist = matcheslist.OrderBy(x => x.Field).ToList();
                //COMPID = 1 for DEMO
                var competition = (from c in db.Competitions where c.CompID == compid select c).FirstOrDefault();
                competition.field1preferred = matcheslist[0].TeamNumber;
                RunningComp[compid].OnDeck[1].TeamNumber = matcheslist[0].TeamNumber;
                competition.field2preferred = matcheslist[1].TeamNumber;
                RunningComp[compid].OnDeck[2].TeamNumber = matcheslist[1].TeamNumber;
                competition.field3preferred = matcheslist[2].TeamNumber;
                RunningComp[compid].OnDeck[3].TeamNumber = matcheslist[2].TeamNumber;
                competition.field4preferred = matcheslist[3].TeamNumber;
                RunningComp[compid].OnDeck[4].TeamNumber = matcheslist[3].TeamNumber;
                competition.field5preferred = matcheslist[4].TeamNumber;
                RunningComp[compid].OnDeck[5].TeamNumber = matcheslist[4].TeamNumber;
                competition.field6preferred = matcheslist[5].TeamNumber;
                RunningComp[compid].OnDeck[6].TeamNumber = matcheslist[5].TeamNumber;
                competition.validmatch1 = matcheslist[0].Valid;
                competition.validmatch2 = matcheslist[1].Valid;
                competition.validmatch3 = matcheslist[2].Valid;
                competition.validmatch4 = matcheslist[3].Valid;
                competition.validmatch5 = matcheslist[4].Valid;
                competition.validmatch6 = matcheslist[5].Valid;
                db.Competitions.Update(competition);
                db.SaveChanges();
                Clients.Clients(compGroups[compid]).SendAsync("availableSelections", matcheslist[0].TeamNumber, matcheslist[1].TeamNumber, matcheslist[2].TeamNumber, matcheslist[3].TeamNumber, matcheslist[4].TeamNumber, matcheslist[5].TeamNumber, 1);
                Clients.Clients(compGroups[compid]).SendAsync("validateSelections", matcheslist[0].Valid, matcheslist[1].Valid, matcheslist[2].Valid, matcheslist[3].Valid, matcheslist[4].Valid, matcheslist[5].Valid);
                RunningComp[compid].OnDeck[1].TeamID = 0;
                RunningComp[compid].OnDeck[2].TeamID = 0;
                RunningComp[compid].OnDeck[3].TeamID = 0;
                RunningComp[compid].OnDeck[4].TeamID = 0;
                RunningComp[compid].OnDeck[5].TeamID = 0;
                RunningComp[compid].OnDeck[6].TeamID = 0;
                RunningComp[compid].OnDeck[1].Rerun = matcheslist[0].Rerun;
                RunningComp[compid].OnDeck[2].Rerun = matcheslist[1].Rerun;
                RunningComp[compid].OnDeck[3].Rerun = matcheslist[2].Rerun;
                RunningComp[compid].OnDeck[4].Rerun = matcheslist[3].Rerun;
                RunningComp[compid].OnDeck[5].Rerun = matcheslist[4].Rerun;
                RunningComp[compid].OnDeck[6].Rerun = matcheslist[5].Rerun;
                RunningComp[compid].OnDeck[1].Test = matcheslist[0].Test;
                RunningComp[compid].OnDeck[2].Test = matcheslist[1].Test;
                RunningComp[compid].OnDeck[3].Test = matcheslist[2].Test;
                RunningComp[compid].OnDeck[4].Test = matcheslist[3].Test;
                RunningComp[compid].OnDeck[5].Test = matcheslist[4].Test;
                RunningComp[compid].OnDeck[6].Test = matcheslist[5].Test;
                RunningComp[compid].OnDeck[1].Round = matcheslist[0].Round;
                RunningComp[compid].OnDeck[2].Round = matcheslist[1].Round;
                RunningComp[compid].OnDeck[3].Round = matcheslist[2].Round;
                RunningComp[compid].OnDeck[4].Round = matcheslist[3].Round;
                RunningComp[compid].OnDeck[5].Round = matcheslist[4].Round;
                RunningComp[compid].OnDeck[6].Round = matcheslist[5].Round;
                if (RunningComp[compid].OnDeck[1].TeamNumber.Contains("-"))
                {
                    var FieldsQuery1 = RunningComp[compid].OnDeck[1].TeamNumber.Split("-");
                    RunningComp[compid].OnDeck[1].TeamID = (from x in db.StudentTeams where x.TeamNumberBranch.ToString() == FieldsQuery1[0] && x.TeamNumberSpecific.ToString() == FieldsQuery1[1] select x).FirstOrDefault().TeamID;
                }
                if (RunningComp[compid].OnDeck[2].TeamNumber.Contains("-"))
                {
                    var FieldsQuery1 = RunningComp[compid].OnDeck[2].TeamNumber.Split("-");
                    RunningComp[compid].OnDeck[2].TeamID = (from x in db.StudentTeams where x.TeamNumberBranch.ToString() == FieldsQuery1[0] && x.TeamNumberSpecific.ToString() == FieldsQuery1[1] select x).FirstOrDefault().TeamID;
                }
                if (RunningComp[compid].OnDeck[3].TeamNumber.Contains("-"))
                {
                    var FieldsQuery1 = RunningComp[compid].OnDeck[3].TeamNumber.Split("-");
                    RunningComp[compid].OnDeck[3].TeamID = (from x in db.StudentTeams where x.TeamNumberBranch.ToString() == FieldsQuery1[0] && x.TeamNumberSpecific.ToString() == FieldsQuery1[1] select x).FirstOrDefault().TeamID;
                }
                if (RunningComp[compid].OnDeck[4].TeamNumber.Contains("-"))
                {
                    var FieldsQuery1 = RunningComp[compid].OnDeck[4].TeamNumber.Split("-");
                    RunningComp[compid].OnDeck[4].TeamID = (from x in db.StudentTeams where x.TeamNumberBranch.ToString() == FieldsQuery1[0] && x.TeamNumberSpecific.ToString() == FieldsQuery1[1] select x).FirstOrDefault().TeamID;
                }
                if (RunningComp[compid].OnDeck[5].TeamNumber.Contains("-"))
                {
                    var FieldsQuery1 = RunningComp[compid].OnDeck[5].TeamNumber.Split("-");
                    RunningComp[compid].OnDeck[5].TeamID = (from x in db.StudentTeams where x.TeamNumberBranch.ToString() == FieldsQuery1[0] && x.TeamNumberSpecific.ToString() == FieldsQuery1[1] select x).FirstOrDefault().TeamID;
                }
                if (RunningComp[compid].OnDeck[6].TeamNumber.Contains("-"))
                {
                    var FieldsQuery1 = RunningComp[compid].OnDeck[6].TeamNumber.Split("-");
                    RunningComp[compid].OnDeck[6].TeamID = (from x in db.StudentTeams where x.TeamNumberBranch.ToString() == FieldsQuery1[0] && x.TeamNumberSpecific.ToString() == FieldsQuery1[1] select x).FirstOrDefault().TeamID;
                }
            }
            
        }
        public async System.Threading.Tasks.Task UpdateUserRoleAsync(string UserName, string RoleName, int compid)
        {
            bool delete = await userManager.IsInRoleAsync(context.Users.Where(u => u.UserName == UserName).FirstOrDefault(), RoleName);
            if (delete == true)
            {
                await userManager.RemoveFromRoleAsync(context.Users.Where(u => u.UserName == UserName).FirstOrDefault(), RoleName);
            }
            else
            {
                await userManager.AddToRoleAsync(context.Users.Where(u => u.UserName == UserName).FirstOrDefault(), RoleName);
            }
            //await Clients.Clients(compGroups[compid]).SendAsync("reloadUsers");
            //JUST FOR TESTING USE ALL
            await Clients.All.SendAsync("reloadUsers");
        }
        public async void AddUsersToComp(string[] users, int compid)
        {
            foreach(var userid in users)
            {
                var result = userManager.AddToRoleAsync(context.Users.Where(u => u.Id == userid).FirstOrDefault(), "Comp" + compid.ToString()).Result;
            }
            await Clients.All.SendAsync("reloadUsers");
        }
        public async void RemoveUserFromComp(string username, int compid)
        {
            var result = userManager.RemoveFromRoleAsync(context.Users.Where(u => u.UserName == username).FirstOrDefault(), "Comp" + compid.ToString()).Result;
            await Clients.All.SendAsync("reloadUsers");
        }
        public async void GenerateUserAccounts(int compid)
        {
            var ifroleexists = userManager.FindByNameAsync("Judge_" + compid.ToString() + "-1").Result;
            if(ifroleexists == null)
            {
                List<ClientSeededAccounts> clientSeededAccounts = new List<ClientSeededAccounts>();
                var judge1 = new ApplicationUser();
                var judge2 = new ApplicationUser();
                var judge3 = new ApplicationUser();
                var judge4 = new ApplicationUser();
                var judge5 = new ApplicationUser();
                var judge6 = new ApplicationUser();
                judge1.UserName = "Judge_" + compid.ToString() + "-1";
                judge2.UserName = "Judge_" + compid.ToString() + "-2";
                judge3.UserName = "Judge_" + compid.ToString() + "-3";
                judge4.UserName = "Judge_" + compid.ToString() + "-4";
                judge5.UserName = "Judge_" + compid.ToString() + "-5";
                judge6.UserName = "Judge_" + compid.ToString() + "-6";
                judge1.NormalizedUserName = "JUDGE_" + compid.ToString() + "-1";
                judge2.NormalizedUserName = "JUDGE_" + compid.ToString() + "-2";
                judge3.NormalizedUserName = "JUDGE_" + compid.ToString() + "-3";
                judge4.NormalizedUserName = "JUDGE_" + compid.ToString() + "-4";
                judge5.NormalizedUserName = "JUDGE_" + compid.ToString() + "-5";
                judge6.NormalizedUserName = "JUDGE_" + compid.ToString() + "-6";
                judge1.Email = "JUDGE" + compid.ToString() + "-1@robofest.judge";
                judge2.Email = "JUDGE" + compid.ToString() + "-2@robofest.judge";
                judge3.Email = "JUDGE" + compid.ToString() + "-3@robofest.judge";
                judge4.Email = "JUDGE" + compid.ToString() + "-4@robofest.judge";
                judge5.Email = "JUDGE" + compid.ToString() + "-5@robofest.judge";
                judge6.Email = "JUDGE" + compid.ToString() + "-6@robofest.judge";
                judge1.NormalizedEmail = "JUDGE" + compid.ToString() + "-1@robofest.judge";
                judge2.NormalizedEmail = "JUDGE" + compid.ToString() + "-2@robofest.judge";
                judge3.NormalizedEmail = "JUDGE" + compid.ToString() + "-3@robofest.judge";
                judge4.NormalizedEmail = "JUDGE" + compid.ToString() + "-4@robofest.judge";
                judge5.NormalizedEmail = "JUDGE" + compid.ToString() + "-5@robofest.judge";
                judge6.NormalizedEmail = "JUDGE" + compid.ToString() + "-6@robofest.judge";
                judge1.Id = Guid.NewGuid().ToString();
                judge2.Id = Guid.NewGuid().ToString();
                judge3.Id = Guid.NewGuid().ToString();
                judge4.Id = Guid.NewGuid().ToString();
                judge5.Id = Guid.NewGuid().ToString();
                judge6.Id = Guid.NewGuid().ToString();
                string Judge1Pass = "J!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                string Judge2Pass = "J!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                string Judge3Pass = "J!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                string Judge4Pass = "J!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                string Judge5Pass = "J!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                string Judge6Pass = "J!" + Guid.NewGuid().ToString("n").Substring(0, 8);
                _ = userManager.CreateAsync(judge1, Judge1Pass).Result;
                clientSeededAccounts.Add(new ClientSeededAccounts() { Username = judge1.UserName, GeneratedPassword = Judge1Pass });
                _ = userManager.CreateAsync(judge2, Judge2Pass).Result;
                clientSeededAccounts.Add(new ClientSeededAccounts() { Username = judge2.UserName, GeneratedPassword = Judge2Pass });
                _ = userManager.CreateAsync(judge3, Judge3Pass).Result;
                clientSeededAccounts.Add(new ClientSeededAccounts() { Username = judge3.UserName, GeneratedPassword = Judge3Pass });
                _ = userManager.CreateAsync(judge4, Judge4Pass).Result;
                clientSeededAccounts.Add(new ClientSeededAccounts() { Username = judge4.UserName, GeneratedPassword = Judge4Pass });
                _ = userManager.CreateAsync(judge5, Judge5Pass).Result;
                clientSeededAccounts.Add(new ClientSeededAccounts() { Username = judge5.UserName, GeneratedPassword = Judge5Pass });
                _ = userManager.CreateAsync(judge6, Judge6Pass).Result;
                clientSeededAccounts.Add(new ClientSeededAccounts() { Username = judge6.UserName, GeneratedPassword = Judge6Pass });


                _ = userManager.AddToRoleAsync(judge1, "Comp" + compid.ToString()).Result;
                _ = userManager.AddToRoleAsync(judge1, compid.ToString() + "-Judge").Result;
                _ = userManager.AddToRoleAsync(judge2, "Comp" + compid.ToString()).Result;
                _ = userManager.AddToRoleAsync(judge2, compid.ToString() + "-Judge").Result;
                _ = userManager.AddToRoleAsync(judge3, "Comp" + compid.ToString()).Result;
                _ = userManager.AddToRoleAsync(judge3, compid.ToString() + "-Judge").Result;
                _ = userManager.AddToRoleAsync(judge4, "Comp" + compid.ToString()).Result;
                _ = userManager.AddToRoleAsync(judge4, compid.ToString() + "-Judge").Result;
                _ = userManager.AddToRoleAsync(judge5, "Comp" + compid.ToString()).Result;
                _ = userManager.AddToRoleAsync(judge5, compid.ToString() + "-Judge").Result;
                _ = userManager.AddToRoleAsync(judge6, "Comp" + compid.ToString()).Result;
                _ = userManager.AddToRoleAsync(judge6, compid.ToString() + "-Judge").Result;


                var csvtopassback = "Username,Generated Password" + Environment.NewLine;
                foreach(var account in clientSeededAccounts)
                {
                    csvtopassback = csvtopassback + account.Username + "," + account.GeneratedPassword + Environment.NewLine;
                }

                await Clients.All.SendAsync("returnedAccounts", csvtopassback);


            }
            else
            {
                await Clients.All.SendAsync("returnedAccountsFailed");
            }
        }
        public void GoToRC(string teamnum)
        {
            var teamnumbereach = teamnum.Split("-");
            var TeamID = (from t in db.StudentTeams where t.TeamNumberBranch.ToString() == teamnumbereach[0] && t.TeamNumberSpecific.ToString() == teamnumbereach[1] select t).FirstOrDefault();
            Clients.All.SendAsync("sendJudgesToPage", TeamID.TeamID);
        }
        public void EV3Connection(int fieldNum, int compid)
        {
            //ADDTOPAGH
            RunningComp[compid].Fields[fieldNum].FieldConnectionID = Context.ConnectionId;
            Clients.Client(Context.ConnectionId).SendAsync("ev3ConnectionSuccessful");
        }
        public void JudgeClientConnection(int compid)
        {
            int[] TeamIDs = { RunningComp[compid].OnDeck[1].TeamID, RunningComp[compid].OnDeck[2].TeamID, RunningComp[compid].OnDeck[3].TeamID, RunningComp[compid].OnDeck[4].TeamID, RunningComp[compid].OnDeck[5].TeamID, RunningComp[compid].OnDeck[6].TeamID };
            string[] TeamNumbers = { RunningComp[compid].OnDeck[1].TeamNumber, RunningComp[compid].OnDeck[2].TeamNumber, RunningComp[compid].OnDeck[3].TeamNumber, RunningComp[compid].OnDeck[4].TeamNumber, RunningComp[compid].OnDeck[5].TeamNumber, RunningComp[compid].OnDeck[6].TeamNumber };
            int[] Rounds = { RunningComp[compid].OnDeck[1].Round, RunningComp[compid].OnDeck[2].Round, RunningComp[compid].OnDeck[3].Round, RunningComp[compid].OnDeck[4].Round, RunningComp[compid].OnDeck[5].Round, RunningComp[compid].OnDeck[6].Round };
            bool[] Reruns = { RunningComp[compid].OnDeck[1].Rerun, RunningComp[compid].OnDeck[2].Rerun, RunningComp[compid].OnDeck[3].Rerun, RunningComp[compid].OnDeck[4].Rerun, RunningComp[compid].OnDeck[5].Rerun, RunningComp[compid].OnDeck[6].Rerun };
            bool[] Tests = { RunningComp[compid].OnDeck[1].Test, RunningComp[compid].OnDeck[2].Test, RunningComp[compid].OnDeck[3].Test, RunningComp[compid].OnDeck[4].Test, RunningComp[compid].OnDeck[5].Test, RunningComp[compid].OnDeck[6].Test };
            var competition = (from c in db.Competitions where c.CompID == compid select c).FirstOrDefault();
            bool[] validate = { competition.validmatch1, competition.validmatch2, competition.validmatch3, competition.validmatch4, competition.validmatch5, competition.validmatch6 };
            Clients.Client(Context.ConnectionId).SendAsync("fieldDefaults", TeamIDs, TeamNumbers, Rounds, Reruns, Tests, validate);
            Clients.Client(Context.ConnectionId).SendAsync("changeJudgeLock", RunningComp[compid].JudgesLocked);
        }
        public void ScoreKeeperStatusSet(int status, int compid)
        {
            //FIX
            RunningComp[compid].ScorekeeperStatus = status;
            Clients.Clients(compGroups[compid]).SendAsync("scorekeeperStatus", RunningComp[compid].ScorekeeperStatus);
        }
        public void LockJudges(bool AllowStatus, int compid)
        {
            RunningComp[compid].JudgesLocked = AllowStatus;
            Clients.Clients(compGroups[compid]).SendAsync("changeJudgeLock", RunningComp[compid].JudgesLocked);
        }
        public void ConfirmFieldCompletion(int Zone)
        {
            Clients.All.SendAsync("zoneGot", Zone);
        }
        public void LiveField(int Zone)
        {
            Clients.All.SendAsync("currentZone", Zone);

        }
        public void GetFieldData(int compid)
        {
            Clients.Client(Context.ConnectionId).SendAsync("initFieldView", 1, RunningComp[compid].Fields[1].Status, 0, RunningComp[compid].Fields[1].TeamNumber, true, true, "");
            Clients.Client(Context.ConnectionId).SendAsync("initFieldView", 2, RunningComp[compid].Fields[2].Status, 0, RunningComp[compid].Fields[2].TeamNumber, true, true, "");
            Clients.Client(Context.ConnectionId).SendAsync("initFieldView", 3, RunningComp[compid].Fields[3].Status, 0, RunningComp[compid].Fields[3].TeamNumber, true, true, "");
            Clients.Client(Context.ConnectionId).SendAsync("initFieldView", 4, RunningComp[compid].Fields[4].Status, 0, RunningComp[compid].Fields[4].TeamNumber, true, true, "");
            Clients.Client(Context.ConnectionId).SendAsync("initFieldView", 5, RunningComp[compid].Fields[5].Status, 0, RunningComp[compid].Fields[5].TeamNumber, true, true, "");
            Clients.Client(Context.ConnectionId).SendAsync("initFieldView", 6, RunningComp[compid].Fields[6].Status, 0, RunningComp[compid].Fields[6].TeamNumber, true, true, "");
        }
        public async Task AppLogin(string UserName, string Password, int compid)
        {
            
            var usertosignin = context.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            if(usertosignin != null)
            {
                
                var loginResult = _signInManager.CheckPasswordSignInAsync(usertosignin, Password, false);
                if (loginResult.Result.Succeeded)
                {
                    if(userManager.IsInRoleAsync(usertosignin, "Judge_"+ compid.ToString()+"-1").Result || userManager.IsInRoleAsync(usertosignin, "Judge_" + compid.ToString() + "-2").Result || userManager.IsInRoleAsync(usertosignin, "Judge_" + compid.ToString() + "-3").Result || userManager.IsInRoleAsync(usertosignin, "Judge_" + compid.ToString() + "-4").Result || userManager.IsInRoleAsync(usertosignin, "Judge_" + compid.ToString() + "-5").Result || userManager.IsInRoleAsync(usertosignin, "Judge_" + compid.ToString() + "-6").Result || userManager.IsInRoleAsync(usertosignin, "Main").Result)
                    {
                        await Clients.Client(Context.ConnectionId).SendAsync("authAccept");
                        var auth = new RoAuthUser();
                        //CHANGE
                        auth.CompID = 1;
                        var user = context.Users.Where(x => x.UserName == UserName).FirstOrDefault();
                        await Clients.Client(Context.ConnectionId).SendAsync("authProgress", 20);
                        var userwithroles = context.UserRoles.Where(x => x.UserId == user.Id).ToList();
                        await Clients.Client(Context.ConnectionId).SendAsync("authProgress", 35);
                        foreach (var roletoadd in userwithroles)
                        {
                            auth.Roles.Add(context.Roles.Where(x => x.Id == roletoadd.RoleId).FirstOrDefault());
                        }
                        await Clients.Client(Context.ConnectionId).SendAsync("authProgress", 50);
                        auth.Username = UserName;
                        await Clients.Client(Context.ConnectionId).SendAsync("authProgress", 65);
                        auth.LastSeen = "Logging in...";
                        await Clients.Client(Context.ConnectionId).SendAsync("authProgress", 70);
                        string authToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        await Clients.Client(Context.ConnectionId).SendAsync("authProgress", 75);
                        appAuthUsers.Add(authToken, auth);
                        await Clients.Client(Context.ConnectionId).SendAsync("authProgress", 80);
                        await Clients.Client(Context.ConnectionId).SendAsync("authSucc", authToken, appSessionID);
                    }
                    else
                    {
                        await Clients.Client(Context.ConnectionId).SendAsync("authRoleFail");
                    }
                    
                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("authFail");
                }
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("authFail");
            }
            
            
        }
        public async Task TryTokenLogin(string userToken, string sessionToken)
        {
            if(sessionToken != appSessionID)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("tokenAuthExpired", appSessionID, sessionToken);
            }
            else
            {
                if (appAuthUsers.ContainsKey(userToken))
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("tokenAuthSucc");
                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("tokenAuthInvalid");
                }
            }
        }
        public async Task CheckSignalRHub()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("signalRConnected");
        }
        public async Task GetTeamsUpNow(int compid)
        {
            var listoffields = new List<StaticField>();
            listoffields.Add(RunningComp[compid].Fields[1]);
            listoffields.Add(RunningComp[compid].Fields[2]);
            listoffields.Add(RunningComp[compid].Fields[3]);
            listoffields.Add(RunningComp[compid].Fields[4]);
            listoffields.Add(RunningComp[compid].Fields[5]);
            listoffields.Add(RunningComp[compid].Fields[6]);
            await Clients.Client(Context.ConnectionId).SendAsync("teamsUpNow", listoffields);
        }
        public async Task AutoCompleteSchedule(int compid)
        {
            var allteamstodelete = (from x in db.TeamMatches where x.CompID == compid select x).ToList();
            foreach(var team in allteamstodelete)
            {
                db.TeamMatches.Remove(team);
            }
            await db.SaveChangesAsync();
            var teamstoadd = (from t in db.StudentTeams where t.CompID == compid select t).ToList();
            int order = 0;
            foreach(var team in teamstoadd.OrderBy(x => x.TeamNumberBranch).ThenBy(x => x.TeamNumberSpecific))
            {
                order++;
                var newteammatch = new TeamMatch();
                newteammatch.Test = false;
                newteammatch.Rerun = false;
                newteammatch.Completed = false;
                newteammatch.CompID = compid;
                newteammatch.Order = order;
                newteammatch.RoundNum = 1;
                newteammatch.TeamNumber = team.TeamNumberBranch.ToString() + "-" + team.TeamNumberSpecific.ToString();
                await db.TeamMatches.AddAsync(newteammatch);
            }
            await db.SaveChangesAsync();
            foreach (var team in teamstoadd.OrderByDescending(x => x.TeamNumberBranch).ThenByDescending(x => x.TeamNumberSpecific))
            {
                order++;
                var newteammatch = new TeamMatch();
                newteammatch.Test = false;
                newteammatch.Rerun = false;
                newteammatch.Completed = false;
                newteammatch.CompID = compid;
                newteammatch.Order = order;
                newteammatch.RoundNum = 2;
                newteammatch.TeamNumber = team.TeamNumberBranch.ToString() + "-" + team.TeamNumberSpecific.ToString();
                await db.TeamMatches.AddAsync(newteammatch);
            }
            await db.SaveChangesAsync();
            await Clients.Client(Context.ConnectionId).SendAsync("scheduleComplete");
        }
        public async Task appSubmitMatch(string userToken, string sessionToken)
        {

        }
        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }
    }
}