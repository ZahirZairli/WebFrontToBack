//$("#btnLoadMore").on("click", () => {
//    let serviceCount = $("#mainDiv").children().length;
//    fetch("/Services/LoadMore", {
//        method: "GET",
//        bocy:
//        JSON.stringify({
//            skip:serviceCount
//              })
//    })
//        .then(res => res.text())
//        .then(data => {
//            $("#mainDiv").append(data);
//        })
//})
let sercount = $("#element").val();
$("#btnLoadMore").on("click", () => {
    let serviceCount = $("#mainDiv").children().length;
    $.ajax("/Services/LoadMore", {
        method: "GET",
        data: {
            skip: serviceCount
        },
        beforeSend: function () {
            $("#loader").show();
            //return confirm("Are you sure?");
        },
        success: (data) => {
            if (sercount >= serviceCount) {
                $("#btnLoadMore").hide();
            }
                $("#mainDiv").append(data);
                $("#loader").hide();
        },
        error: () => {
            $("#loader").hide();
            alert("Error!");
        } 
    })
})



//$("#btnLoadMore").on("click", () => {
//    let serviceCount = $("#mainDiv").children().length;
//    $.ajax("/Services/LoadMore", {
//        method: "Get",
//        data: {
//            skip: serviceCount
//        },  
//        //beforesend: () => {
//        //    $("#loader").show();
//        //},
//        success: (data) => {
//            $("#divMain").append(data);
//        }
//    })
//})



















