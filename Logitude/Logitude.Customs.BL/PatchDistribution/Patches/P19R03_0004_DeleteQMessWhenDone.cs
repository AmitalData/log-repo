using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    public class P19R03_0004_DeleteQMessWhenDone : PatchDistributionBase
    {
        public P19R03_0004_DeleteQMessWhenDone()
            :base("שינוי SP queue_setstatus  מחיקת תור שהסתיים", new DateTime(2019, 12, 2))
        {

        }
        public override List<ScriptDTO> GetDownScripts()
        {
            throw new NotImplementedException();
        }

        public override List<ScriptDTO> GetUpScripts()
        {
            int ScriptCount = 0;
            return new List<ScriptDTO>()
            {
                new ScriptDTO()
                {
                    ScriptCounter = ScriptCount++,
                    SqlScript =
@"CREATE OR REPLACE PROCEDURE queue_setstatus (
    v_messageid   IN NUMBER,
    v_statud      IN NUMBER
) AS
    v_temp   NUMBER(1,0) := 0;
            BEGIN
                BEGIN
        SELECT
            1
        INTO
            v_temp
        FROM
            dual
        WHERE
            EXISTS(
                SELECT
                    id
                FROM
                    queuemessages
                WHERE
                    id = v_messageid
            );

            EXCEPTION
                WHEN OTHERS THEN
            NULL;
            END;

            IF
                v_temp = 1
    THEN
        IF
            (v_statud = 1)
        THEN
            BEGIN
                DELETE FROM queuemessages WHERE
                    id = v_messageid;

            END;
            ELSE
                BEGIN
                UPDATE queuemessages
                    SET
                        status = v_statud,
                        completedatetime = SYSDATE
                WHERE
                    id = v_messageid;
            END;
        END IF;
    END IF;
END;"
                },
            };
        }
    }
}
