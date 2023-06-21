using UniANPR.Models;
using Newtonsoft.Json;

namespace UniANPR.Utility
{
    public class TheosAPI : IDisposable
    {
        private readonly HttpClient httpClient;
        private const string URL = "https://inf-90d32d28-70d9-49ae-b6e7-03dd2552b374-no4xvrhsfq-uc.a.run.app/detect"; // copy and paste your URL here
        private const string FALLBACK_URL = ""; // copy and paste your fallback URL here

        public TheosAPI()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<NumberPlate>> Detect(FileInfo imageFile, string url = URL, float confThres = 0.25f, float iouThres = 0.45f, string ocrModel = null, string ocrClasses = null, string ocrLanguage = null, int retries = 10, int delay = 0)
        {
            List<NumberPlate> rtnObjects = new List<NumberPlate>();
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(File.OpenRead(imageFile.FullName)), "image", imageFile.Name);
            formData.Add(new StringContent(confThres.ToString()), "conf_thres");
            formData.Add(new StringContent(iouThres.ToString()), "iou_thres");

            if (ocrModel != null)
            {
                formData.Add(new StringContent(ocrModel), "ocr_model");
            }
            if (ocrClasses != null)
            {
                formData.Add(new StringContent(ocrClasses), "ocr_classes");
            }
            if (ocrLanguage != null)
            {
                formData.Add(new StringContent(ocrLanguage), "ocr_language");
            }

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, formData);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();
                
                // Handle the response data
                rtnObjects = JsonConvert.DeserializeObject<List<NumberPlate>>(responseData);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == 0 || Convert.ToInt32(ex.StatusCode) == 413)
                {
                    throw new Exception("image too large, please select an image smaller than 25MB.");
                }
                else if (Convert.ToInt32(ex.StatusCode)  == 403)
                {
                    throw new Exception("you reached your monthly requests limit. Upgrade your plan to unlock unlimited requests.");
                }
                else
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception)
            {
                if (retries > 0)
                {
                    if (delay > 0)
                    {
                        await Task.Delay(delay * 1000);
                    }
                    await Detect(imageFile, url: FALLBACK_URL ?? URL, confThres: 0.25f, iouThres: 0.45f, retries: retries - 1, delay: 2);
                }
                else
                {
                    // Handle the case when retries are exhausted
                }
            }
            return rtnObjects;
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
