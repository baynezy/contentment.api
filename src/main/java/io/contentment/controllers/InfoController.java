package io.contentment.controllers;

import io.contentment.api.InfoApi;
import io.contentment.model.ApiInfo;
import io.swagger.annotations.ApiOperation;
import io.swagger.annotations.ApiResponse;
import io.swagger.annotations.ApiResponses;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class InfoController implements InfoApi {

	@ApiOperation(value = "", nickname = "getInfo", notes = "Information for the current status of the API", response = ApiInfo.class, tags={  })
	@ApiResponses(value = {
		@ApiResponse(code = 200, message = "API Information", response = ApiInfo.class) })
	@RequestMapping(value = "/info",
		produces = { "application/json" },
		method = RequestMethod.GET)
	public ResponseEntity<ApiInfo> getInfo() {
		return new ResponseEntity<>(new ApiInfo(), HttpStatus.OK);
	}
}
