using gymApp.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace gymApp.pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HiitPage : ContentPage
	{
        public Hiit hiit = new Hiit();
        public bool timerStatus;
        public int actionTime = 0;
        public HiitPage ()
		{
			InitializeComponent();  
        }
        
        public static void StartTimer(TimeSpan interval, Func<bool> callback) { }
        //xamarin timer, PCL version is not included

        private void ShowTotalTime()
        {
            int totalTime = 0;
            if(hiit.Work != 0 && hiit.Rest != 0) {
                totalTime = (hiit.Work + hiit.Rest) * hiit.Rounds - hiit.Rest + hiit.Prep;
            }
            hiit.totalTime = totalTime;
            timer.Text = ReturnTimeInFormat(totalTime); 

        }

        private string ReturnTimeInFormat(int timeParameter)
        {
            int minute =0;
            int second = 0;
            string timeFormated;
            minute = timeParameter / 60;
            second= timeParameter - minute * 60;
            timeFormated = TimeAdjustment(minute) + ":" + TimeAdjustment(second);
            if(timeParameter != hiit.totalTime)
            {
                ShowTotalTime();
            }
            return timeFormated;
        }

        private string TimeAdjustment(int timeParameter)
        {
            string time;
            if(timeParameter <= 9)
            {
                time = "0" + timeParameter.ToString();
            }
            else
            {
                time = timeParameter.ToString();
            }
            return time;
        }

        private void VisibilityButton(Button buttonName, int timeParameter)
        {
            if(timeParameter <= 1)
            {
                buttonName.IsVisible = false;
            }
            else
            {
                buttonName.IsVisible = true;
            }
        }

        private void prepTimePlus_Clicked(object sender, EventArgs e)
        {
            hiit.Prep = hiit.Prep + 5;
            prepTime.Text = ReturnTimeInFormat(hiit.Prep);
            VisibilityButton(prepTimeMinus,hiit.Prep);
        }
        private void prepTimeMinus_Clicked(object sender, EventArgs e)
        {
            hiit.Prep = hiit.Prep - 5;
            prepTime.Text = ReturnTimeInFormat(hiit.Prep);
            VisibilityButton(prepTimeMinus, hiit.Prep);
        }

        private void workTimePlus_Clicked(object sender, EventArgs e)
        {
            hiit.Work = hiit.Work + 5;
            workTime.Text = ReturnTimeInFormat(hiit.Work);
            VisibilityButton(workTimeMinus, hiit.Work);
        }
        private void workTimeMinus_Clicked(object sender, EventArgs e)
        {
            hiit.Work = hiit.Work - 5;
            workTime.Text = ReturnTimeInFormat(hiit.Work);
            VisibilityButton(workTimeMinus, hiit.Work);
        }

        private void restTimePlus_Clicked(object sender, EventArgs e)
        {
            hiit.Rest = hiit.Rest + 5;
            restTime.Text = ReturnTimeInFormat(hiit.Rest);
            VisibilityButton(restTimeMinus, hiit.Rest);
        }
        private void restTimeMinus_Clicked(object sender, EventArgs e)
        {
            hiit.Rest = hiit.Rest - 5;
            restTime.Text = ReturnTimeInFormat(hiit.Rest);
            VisibilityButton(restTimeMinus, hiit.Rest);
        }

        private void roundsTimePlus_Clicked(object sender, EventArgs e)
        {
            hiit.Rounds = hiit.Rounds + 1;
            roundsTime.Text = hiit.Rounds.ToString();
            VisibilityButton(roundsTimeMinus, hiit.Rounds);
            ShowTotalTime();
        }
        private void roundsTimeMinus_Clicked(object sender, EventArgs e)
        {
            hiit.Rounds = hiit.Rounds - 1;
            roundsTime.Text = hiit.Rounds.ToString();
            VisibilityButton(roundsTimeMinus, hiit.Rounds);
            ShowTotalTime();
        }

        private void startCounting_Clicked(object sender, EventArgs e)
        {
            if(hiit.totalTime != 0)
            {
                startCounting.Text = "ZAČÍT";
                cancelCouting.Text = "ZASTAVIT";
                gridSettup.IsVisible = false;
                gridCountdown.IsVisible = true;
                timerStatus = true;
                countDownActual.Text = ReturnTimeInFormat(hiit.Prep);
                countDown.Text = ReturnTimeInFormat(hiit.totalTime);
                startCountdown();
            }
            else
            {
                timer.Text = "ZADEJ ČAS";
            }
            
        }
        private int actualTime = 0;
        private void startCountdown()
        {
            action.Text = "PŘIPRAV SE";
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                actualTime++;
                
                if (actualTime <= hiit.totalTime)
                {
                    if(hiit.Prep <= actualTime){
                        actionTime++;
                        SetActualText(actualTime);
                    }
                    else { countDownActual.Text = ReturnTimeInFormat(hiit.Rest - actualTime); }
                    countDown.Text = ReturnTimeInFormat(hiit.totalTime - actualTime);
                    return timerStatus; //continue
                }
                timerStatus = false;
                InformAboutEnd();
                return timerStatus; //not continue
            });
        }

        private void InformAboutEnd()
        {
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("beepEnd.mp3");
            player.Play();
            action.Text = "HOTOVO";
            DefaultHiit();
            action.Text = "SPLNĚNO";
            cancelCouting.Text = "ÚSPĚŠNĚ SPLĚNO";
            countDownActual.Text = "00:00";
            
        }

        private void SetActualText(int actualTime)
        {
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            if (actionTime <= hiit.Work)
            {
                countDownActual.Text = ReturnTimeInFormat(hiit.Work-actionTime+1);
                action.Text = "CVIČ";
                if(hiit.Work - actionTime + 1 < 4)
                {
                    player.Load("beep.mp3");
                    player.Play();
                }
                if (hiit.Work + actionTime == hiit.Work+1) {
                    player.Load("cvic.mp3");
                    player.Play();
                }
            }
            else if(actionTime < hiit.Work + hiit.Rest)
            {
                if(actualTime != hiit.totalTime)
                {
                    countDownActual.Text = ReturnTimeInFormat(hiit.Work + hiit.Rest - actionTime + 1);
                    action.Text = "PAUZA";
                }
                if (hiit.Work + hiit.Rest - actionTime + 1 < 4)
                {
                    player.Load("beep.mp3");
                    player.Play();
                }
                if (hiit.Rest + 1 == actionTime)
                {
                    if (actualTime != hiit.totalTime)
                    {
                        player.Load("pauza.mp3");
                        player.Play();
                    }
                }

            }
            else {
                countDownActual.Text = ReturnTimeInFormat(hiit.Work + hiit.Rest - actionTime+1);
                actionTime = 0;
                action.Text = "PAUZA";
                player.Load("beep.mp3");
                player.Play();
            }
        }
        private void DefaultHiit()
        {
            actualTime = 0;
            hiit.Work = 0;
            hiit.Prep = 0;
            hiit.Rest = 0;
            hiit.totalTime = 0;
            hiit.Rounds = 1;

            actionTime = 0;
            actualTime = 0;

            roundsTime.Text = hiit.Rounds.ToString();
            workTime.Text = ReturnTimeInFormat(hiit.Work);
            prepTime.Text = ReturnTimeInFormat(hiit.Work);
            restTime.Text = ReturnTimeInFormat(hiit.Work);
            timer.Text = ReturnTimeInFormat(hiit.totalTime);

            VisibilityButton(workTimeMinus, hiit.Work);
            VisibilityButton(prepTimeMinus, hiit.Prep);
            VisibilityButton(restTimeMinus, hiit.Rest);
            VisibilityButton(roundsTimeMinus, hiit.Rounds);

            action.Text = "PŘIPRAV SE";
            countDownActual.Text = "00:00";
            countDown.Text = "00:00";
        }
        private void cancelCouting_Clicked(object sender, EventArgs e)
        {
            DefaultHiit();
            timerStatus = false; //stop counting
            gridSettup.IsVisible = true;
            gridCountdown.IsVisible = false;
            
          
        }
    }
}