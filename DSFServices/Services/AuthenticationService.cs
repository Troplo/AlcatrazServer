using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Connection;
using QNetZ.DDL;
using QNetZ.Interfaces;
using RDVServices.DDL.Models;
using RVConnectionData = DSFServices.DDL.Models.RVConnectionData;

namespace DSFServices.Services
{
    [RMCService(RMCProtocolId.NexusOnly, Name = "SimpleAuthenticationProtocol")]
    public class AuthenticationService : RMCServiceBase
    {
        [RMCMethod(0, Name = "LoginWithToken_V2")]
        public RMCResult LoginWithToken(string strToken, string strOnlineKey, ClientVersionInfo clientVersionInfo, RVConnectionData rvConnectionData)
        {
            var client = Context.Client;
            var playerInfo = NetworkPlayers.CreatePlayerInfo(client);

            using (var db = RDVServices.DBHelper.GetDbContext())
            {
                var sessionToken = db.SessionTokens
                    .Include(t => t.User)
                    .FirstOrDefault(t => t.Id == strToken);

                if (sessionToken != null && sessionToken.User != null)
                {
                    playerInfo.PID = sessionToken.User.Id;
                    playerInfo.Name = sessionToken.User.PlayerNickName;
                }
                else
                { 
                    QLog.WriteLine(1, $"[QRV] LoginWithToken_V2: login failed {strToken}");
            
                    return Error((int)ErrorCode.Authentication_TokenExpired);
                }
            }

            playerInfo.AccountId = $"00000000-0000-0000-0000-{playerInfo.PID:D12}";

            client.PlayerInfo = playerInfo;
            playerInfo.Client = client;

            QLog.WriteLine(1, $"[QRV] LoginWithToken_V2: logged in as PID={playerInfo.PID} name={playerInfo.Name}. Token: {strToken}, OnlineKey: {strOnlineKey}, ClientVersion: {clientVersionInfo}");

            var m = new MemoryStream();

            m.Write(new byte[] { 0,0,0,0, 0,0, 0,0, 0,0, 0,0,0,0,0,1 }, 0, 16);

            var host = QConfiguration.Instance.ServiceURLHostName
                       ?? QConfiguration.Instance.ServerBindAddress
                       ?? "127.0.0.1";
            var port = QConfiguration.Instance.RDVServerPort;
            var secureUrl = $"prudp:/address={host};port={port};CID=1;PID={playerInfo.PID};sid=1;stream=3;type=2";

            Helper.WriteString(m, secureUrl);  // m_urlRegularProtocols
            Helper.WriteU32(m, 0);             // m_lstSpecialProtocols (empty)
            Helper.WriteString(m, secureUrl);  // m_urlSpecialProtocols

            return Result(m.ToArray());
        }


        [RMCMethod(0, Name = "Register_V1")]
        public RMCResult Register(List<string> vecMyURLs)
        {
            var rdvConnectionUrl = new StationURL(vecMyURLs.Last().ToString());
            rdvConnectionUrl.Address = Context.Client.Endpoint.Address.ToString();
            rdvConnectionUrl["type"] = 3;
            QLog.WriteLine(1, $"[QRV] Register_V1: client {Context.Client.PlayerInfo.Name} registered RDV URL {rdvConnectionUrl}");

            var result = new RegisterResult()
            {
                pidConnectionID = Context.Client.PlayerInfo.RVCID,
                retval = (int)ErrorCode.Core_NoError,
                urlPublic = rdvConnectionUrl
            };

            return Result(result);
        }
    }
}
