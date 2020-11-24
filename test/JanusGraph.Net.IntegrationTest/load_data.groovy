// Copyright 2019 JanusGraph Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License

g = traversal().withRemote('conf/remote-graph.properties')

g.addV('titan').property('name', 'saturn').property('age', 10000).as('saturn').
  addV('location').property('name', 'sky').as('sky').
  addV('location').property('name', 'sea').as('sea').
  addV('god').property('name', 'jupiter').property('age', 5000).as('jupiter').
  addV('god').property('name', 'neptune').property('age', 4500).as('neptune').
  addV('demigod').property('name', 'hercules').property('age', 30).as('hercules').
  addV('human').property('name', 'alcmene').property('age', 45).as('alcmene').
  addV('god').property('name', 'pluto').property('age', 4000).as('pluto').
  addV('monster').property('name', 'nemean').as('nemean').
  addV('monster').property('name', 'hydra').as('hydra').
  addV('monster').property('name', 'cerberus').as('cerberus').
  addV('location').property('name', 'tartarus').as('tartarus').
  addE('father').from('jupiter').to('saturn').
  addE('lives').from('jupiter').to('sky').property('reason', 'loves fresh breezes').
  addE('brother').from('jupiter').to('neptune').
  addE('brother').from('jupiter').to('pluto').
  addE('lives').from('neptune').to('sea').property('reason', 'loves waves').
  addE('brother').from('neptune').to('jupiter').
  addE('brother').from('neptune').to('pluto').
  addE('father').from('hercules').to('jupiter').
  addE('mother').from('hercules').to('alcmene').
  addE('battled').from('hercules').to('nemean').property('time', 1).property('place', Geoshape.point(38.1f, 23.7f)).
  addE('battled').from('hercules').to('hydra').property('time', 2).property('place', Geoshape.point(37.7f, 23.9f)).
  addE('battled').from('hercules').to('cerberus').property('time', 12).property('place', Geoshape.point(39f, 22f)).
  addE('brother').from('pluto').to('jupiter').
  addE('brother').from('pluto').to('neptune').
  addE('lives').from('pluto').to('tartarus').property('reason', 'no fear of death').
  addE('pet').from('pluto').to('cerberus').
  addE('lives').from('cerberus').to('tartarus').
  iterate()
