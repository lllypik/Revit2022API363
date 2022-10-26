using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitAPILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit2022API363
{
    class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<Level> Levels { get; private set; } = new List<Level>();
        public List<FamilySymbol> FamilyTypes { get; private set; } = new List<FamilySymbol>();
        public XYZ PointBegin { get; private set; }
        public XYZ PointEnd { get; private set; }
        public DelegateCommand ApplyCommand { get; private set; }
        public FamilySymbol SelectedFamilyType { get; set; }
        public int NumberFamilyTypes { get; set; }
        public Level SelectedLevel { get; set; }


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;

            Levels = LevelData.GetLevels(_commandData);

            FamilyTypes = FamilySymbolData.GetFamilySymbols(_commandData);

            PointBegin = SelectionUtils.GetPoint(_commandData, "Выберите начальную точку", Autodesk.Revit.UI.Selection.ObjectSnapTypes.Endpoints);

            PointEnd = SelectionUtils.GetPoint(_commandData, "Выберите конечную точку", Autodesk.Revit.UI.Selection.ObjectSnapTypes.Endpoints);

            ApplyCommand = new DelegateCommand(OnApplyCommand);

        }

        private void OnApplyCommand()
        {


            XYZ pointInsert = PointBegin;

            for (int i = 0; i < NumberFamilyTypes; i++)
            {
                pointInsert = pointInsert + (PointEnd - PointBegin) / (NumberFamilyTypes + 1);

                FamilyInstanceData.CreateFamilyInstance(_commandData, SelectedFamilyType, pointInsert, SelectedLevel);
            }

            RaiseCloseReqest();

        }

        public event EventHandler CloseReqest;

        private void RaiseCloseReqest()
        {
            CloseReqest?.Invoke(this, EventArgs.Empty);
        }
    }
}
