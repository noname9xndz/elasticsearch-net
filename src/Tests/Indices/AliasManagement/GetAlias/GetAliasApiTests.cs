﻿using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using FluentAssertions;
using Nest;
using Tests.Framework;
using Tests.Framework.Integration;
using Xunit;

namespace Tests.Indices.AliasManagement.GetAlias
{
	[Collection(IntegrationContext.ReadOnly)]
	public class GetAliasApiTests : ApiIntegrationTestBase<IGetAliasesResponse, IGetAliasRequest, GetAliasDescriptor, GetAliasRequest>
	{
		private readonly static Names Names = Infer.Names("alias, x", "y");

		public GetAliasApiTests(ReadOnlyCluster cluster, EndpointUsage usage) : base(cluster, usage) { }
		
		protected override LazyResponses ClientUsage() => Calls(
			fluent: (client, f) => client.GetAlias(f),
			fluentAsync: (client, f) => client.GetAliasAsync(f),
			request: (client, r) => client.GetAlias(r),
			requestAsync: (client, r) => client.GetAliasAsync(r)
		);

		protected override bool ExpectIsValid => true;
		protected override int ExpectStatusCode => 200;
		protected override HttpMethod HttpMethod => HttpMethod.GET;
		protected override string UrlPath => $"/_all/_alias/alias,x,y";
		protected override void ExpectResponse(IGetAliasesResponse response)
		{
			response.Indices.Should().NotBeNull();
		}
		protected override bool SupportsDeserialization => false;

		protected override Func<GetAliasDescriptor, IGetAliasRequest> Fluent => d=>d
			.AllIndices()
			.Name(Names)
		;
		protected override GetAliasRequest Initializer => new GetAliasRequest(Infer.AllIndices, Names);
	}
}
