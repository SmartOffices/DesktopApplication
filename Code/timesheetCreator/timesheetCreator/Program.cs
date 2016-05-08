///Created by Naorin Hossain
///

using System;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

public class timesheetCreator
{
    static void Main()
    {
        createTimesheet();
    }

    private static void createTimesheet()
    {
        
        Application excel = new Application();
        Workbook wb = excel.Workbooks.Open("C:\\Users\\Raghav\\Desktop\\timesheetCreator\\timesheetCreator\\TimeSheet.xlsx");
        Worksheet ws = (Worksheet)wb.Worksheets[1];

        int currentUser = getEmployee(ws);
        updateSignInOutData(ws, currentUser);
        //calculateHours(ws);
        wb.Close();
    }

    private static int getEmployee(Worksheet ws)
    {
        string name = "Raghav";
        Boolean presence = false;
        int row = 2;
        while (ws.Cells[row, 1].Value2 != null)
        {
            if (ws.Cells[row, 1].Value2 == name)
            {
                presence = true;
                break;
            }
            row++;
        }
        if (!presence)
        {
            ws.Cells[row, 1].Value2 = name;
        }
        return row;
    }

    private static void calculateHours(Worksheet ws)
    {
        int row = 2;
        while (ws.Cells[row, 1].Value2 != null)
        {
            if (ws.Cells[row, 3].Value2 != null)
            {
                ws.Cells[row, 4].Value2 = ws.Cells[row, 3].Value2 - ws.Cells[row, 2].Value2;
            }
            if (ws.Cells[row, 6].Value2 != null)
            {
                ws.Cells[row, 7].Value2 = ws.Cells[row, 6].Value2 - ws.Cells[row, 5].Value2;
            }
            if (ws.Cells[row, 9].Value2 != null)
            {
                ws.Cells[row, 10].Value2 = ws.Cells[row, 9].Value2 - ws.Cells[row, 8].Value2;
            }
            if (ws.Cells[row, 12].Value2 != null)
            {
                ws.Cells[row, 13].Value2 = ws.Cells[row, 12].Value2 - ws.Cells[row, 11].Value2;
            }
            if (ws.Cells[row, 15].Value2 != null)
            {
                ws.Cells[row, 16].Value2 = ws.Cells[row, 15].Value2 - ws.Cells[row, 14].Value2;
            }
            if (ws.Cells[row, 16].Value2 != null)
            {
                ws.Cells[row, 17].Value2 = ws.Cells[row, 4].Value2 + ws.Cells[row, 7].Value2 + ws.Cells[row, 10].Value2 + ws.Cells[row, 13].Value2 + ws.Cells[row, 16].Value2;
                ws.Cells[row, 18].Value2 = 15.69 * ws.Cells[row, 17].Value2;
            }
            row++;
        }
    }

    private static void updateSignInOutData(Worksheet ws, int currentUser)
    {
        int column = 2;
        while (ws.Cells[currentUser, column].Value2 != null)
        {
            column++;
        }
        ws.Cells[currentUser, column].Value2 = DateTime.Now;
        ws.Cells[currentUser, column].NumberFormat = "hh:mm";
    }
}
