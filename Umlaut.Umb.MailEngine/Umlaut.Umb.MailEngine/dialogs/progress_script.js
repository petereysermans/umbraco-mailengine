<script type="text/javascript">
//<![CDATA[

function pbar_asyncUpdate() {
	var pb = eo_GetObject("ProgressBar1");
	if (pb!=null) {
	$.ajax({
	    url: "MailProgress.asmx/HelloWorld",
	    type: "POST",
	    contentType: "application/json; charset=utf-8",
	    data: "{}",
	    dataType: "json",
	    success: function(data) {
	        pb.setValue(data[0]);
	        $("#divStatus").html(data[1]);
	    },
	    error: function(XMLHttpRequest, textStatus, errorThrown) {
	        alert("Error in progress bar update");
	    }
	});			
	}

	window.setTimeout("pbar_asyncUpdate()", 2000);

    return false;
}
$(document).ready(pbar_asyncUpdate);
//]]>
</script>