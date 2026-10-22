using CitizenDB.Entity;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CitizenDB.ExelMaker
{
    public class ExcelCreater
    {
        public ExcelCreater() { }

        public void CreateExcelFile(List<CitizenExcel> CitizensToExcel)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                DefaultExt = "xlsx",
                FileName = $"Citizens_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                using (var package = new ExcelPackage())
                {
                    // Создаем лист с названием
                    var worksheet = package.Workbook.Worksheets.Add("Citizens");

                    // Заголовки столбцов (на русском для удобства)
                    string[] headers = {
                        "Имя гражданина",
                        "Дата рождения",
                        "Пол",
                        "Группа крови",
                        "Телосложение",
                        "Рост (см)",
                        "Размер обуви",
                        "Тип волос",
                        "Цвет глаз",
                        "Адрес",
                        "Работает в"
                    };

                    // Записываем заголовки в первую строку
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Заполняем данными
                    for (int i = 0; i < CitizensToExcel.Count; i++)
                    {
                        var citizen = CitizensToExcel[i];
                        int row = i + 2; // +2 потому что первая строка заголовки, а индексация с 1

                        worksheet.Cells[row, 1].Value = citizen.citizenName ?? "";
                        worksheet.Cells[row, 2].Value = citizen.birthday ?? "";
                        worksheet.Cells[row, 3].Value = citizen.gender ?? "";
                        worksheet.Cells[row, 4].Value = citizen.blood ?? "";
                        worksheet.Cells[row, 5].Value = citizen.build ?? "";
                        worksheet.Cells[row, 6].Value = citizen.heightCM;
                        worksheet.Cells[row, 7].Value = citizen.shoeSize;
                        worksheet.Cells[row, 8].Value = citizen.hairType ?? "";
                        worksheet.Cells[row, 9].Value = citizen.eyeColour ?? "";
                        worksheet.Cells[row, 10].Value = citizen.Address ?? "";
                        worksheet.Cells[row, 11].Value = citizen.workPlace ?? "";
                    }

                    // Автоматически подгоняем ширину столбцов
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Добавляем фильтр
                    worksheet.Cells[1, 1, 1, headers.Length].AutoFilter = true;

                    // Сохраняем файл
                    FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                    package.SaveAs(excelFile);
                }
                
                MessageBox.Show($"Excel файл успешно создан!\nСохранено {CitizensToExcel.Count} записей.\nПуть: {saveFileDialog.FileName}",
                    "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (result == false)
            {
                MessageBox.Show("Сохранение Excel файла отменено.",
                "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else 
            {
                MessageBox.Show("Диалог сохранения был закрыт.",
                   "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
