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
            gridSettup.IsVisible = false;
            gridCountdown.IsVisible = true;
            timerStatus = true;
            countDown.Text = ReturnTimeInFormat(hiit.totalTime);
            startCountdown();
        }

        private void startCountdown()
        {
           /* var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("hallOfFame.mp3");
            player.Play();*/
            int actualTime = 0;
           
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                actualTime++;
                
                if (actualTime <= hiit.totalTime)
                {
                    if(hiit.Prep <= actualTime){
                        actionTime++;
                        action.Text = SetActualText();
                    }
                    countDown.Text = ReturnTimeInFormat(hiit.totalTime - actualTime);
                    return timerStatus; //continue
                }
                timerStatus = false;
                action.Text = "FINISHED";
                return timerStatus; //not continue
            });
        }
        
        private string SetActualText()
        {
            if(actionTime <= hiit.Work)
            {
                return "WORK";
            }
            else if(actionTime < hiit.Work + hiit.Rest)
            {
                return "REST";
            }
            else {
                actionTime = 0;
                return "REST";
            }
        }

        private void cancelCouting_Clicked(object sender, EventArgs e)
        {
            gridSettup.IsVisible = true;
            gridCountdown.IsVisible = false;
            action.Text = "PREP";
            timerStatus = false; //stop counting
        }
    }
}