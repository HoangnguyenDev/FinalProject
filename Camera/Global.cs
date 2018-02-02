using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera
{
    public static class Global
    {
        public const string PATH_FULL_IMAGE = "Picture\\Full\\";
        public const string PATH_PLATE_IMAGE = "Picture\\LPR\\";
        public const string PATH_FACE_IMAGE = "Picture\\Face\\";
        public const string TEXT_DETECT_FACE = "Đang tiến hành nhận dạng khuôn mặt";
        public const string TEXT_DETECT_PLATE = "Đang tiến hành nhận dạng biển số";
        public const string TEXT_WAITING = "Đang chờ xe mới";
        public const string TEXT_SUCCESS = "Thành công";
        public const string TEXT_SAVE = "Đang Lưu";
        public const int WAITING_TIME = 3000;
        //public const int SIZE_WIDTH_SAVE_FACE = 200;
        //public const int SIZE_HEIGHT_SAVE_FACE = 150;
    }
}
