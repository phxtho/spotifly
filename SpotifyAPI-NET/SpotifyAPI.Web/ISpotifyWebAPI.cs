using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyAPI.Web
{
    public interface ISpotifyWebAPI
    {
        #region Search

        SearchItem SearchItems(string q, SearchType type, int limit = 20, int offset = 0, string market = "");

        Task<SearchItem> SearchItemsAsync(string q, SearchType type, int limit = 20, int offset = 0, string market = "");

        SearchItem SearchItemsEscaped(string q, SearchType type, int limit = 20, int offset = 0, string market = "");

        Task<SearchItem> SearchItemsEscapedAsync(string q, SearchType type, int limit = 20, int offset = 0, string market = "");

        #endregion

        #region Albums

        Paging<SimpleTrack> GetAlbumTracks(string id, int limit = 20, int offset = 0, string market = "");

        Task<Paging<SimpleTrack>> GetAlbumTracksAsync(string id, int limit = 20, int offset = 0, string market = "");

        FullAlbum GetAlbum(string id, string market = "");

        Task<FullAlbum> GetAlbumAsync(string id, string market = "");

        SeveralAlbums GetSeveralAlbums(List<string> ids, string market = "");

        Task<SeveralAlbums> GetSeveralAlbumsAsync(List<string> ids, string market = "");

        #endregion

        #region Artists

        FullArtist GetArtist(string id);

        Task<FullArtist> GetArtistAsync(string id);

        SeveralArtists GetRelatedArtists(string id);

        Task<SeveralArtists> GetRelatedArtistsAsync(string id);

        SeveralTracks GetArtistsTopTracks(string id, string country);

        Task<SeveralTracks> GetArtistsTopTracksAsync(string id, string country);

        Paging<SimpleAlbum> GetArtistsAlbums(string id, AlbumType type = AlbumType.All, int limit = 20, int offset = 0, string market = "");

        Task<Paging<SimpleAlbum>> GetArtistsAlbumsAsync(string id, AlbumType type = AlbumType.All, int limit = 20, int offset = 0, string market = "");

        SeveralArtists GetSeveralArtists(List<string> ids);

        Task<SeveralArtists> GetSeveralArtistsAsync(List<string> ids);

        #endregion

        #region Personalization

        Paging<FullTrack> GetUsersTopTracks(TimeRangeType timeRange = TimeRangeType.MediumTerm, int limit = 20, int offest = 0);

        Task<Paging<FullTrack>> GetUsersTopTracksAsync(TimeRangeType timeRange = TimeRangeType.MediumTerm, int limit = 20, int offest = 0);

        Paging<FullArtist> GetUsersTopArtists(TimeRangeType timeRange = TimeRangeType.MediumTerm, int limit = 20, int offest = 0);

        Task<Paging<FullArtist>> GetUsersTopArtistsAsync(TimeRangeType timeRange = TimeRangeType.MediumTerm, int limit = 20, int offest = 0);

        CursorPaging<PlayHistory> GetUsersRecentlyPlayedTracks(int limit = 20, DateTime? after = null,
            DateTime? before = null);

        Task<CursorPaging<PlayHistory>> GetUsersRecentlyPlayedTracksAsync(int limit = 20, DateTime? after = null,
            DateTime? before = null);

        #endregion

        #region Playlist

        Paging<SimplePlaylist> GetUserPlaylists(string userId, int limit = 20, int offset = 0);

        Task<Paging<SimplePlaylist>> GetUserPlaylistsAsync(string userId, int limit = 20, int offset = 0);

        FullPlaylist GetPlaylist(string userId, string playlistId, string fields = "", string market = "");

        FullPlaylist GetPlaylist(string playlistId, string fields = "", string market = "");

        Task<FullPlaylist> GetPlaylistAsync(string playlistId, string fields = "", string market = "");

        Paging<PlaylistTrack> GetPlaylistTracks(string playlistId, string fields = "", int limit = 100, int offset = 0, string market = "");

        FullPlaylist CreatePlaylist(string userId, string playlistName, bool isPublic = true, bool isCollaborative = false, string playlistDescription = "");

        Task<FullPlaylist> CreatePlaylistAsync(string userId, string playlistName, bool isPublic = true, bool isCollaborative = false, string playlistDescription = "");

        ErrorResponse UpdatePlaylist(string playlistId, string newName = null, bool? newPublic = null, bool? newCollaborative = null, string newDescription = null);

        Task<ErrorResponse> UpdatePlaylistAsync(string playlistId, string newName = null, bool? newPublic = null, bool? newCollaborative = null, string newDescription = null);

        ErrorResponse UploadPlaylistImage(string userId, string playlistId, string base64EncodedJpgImage);

        Task<ErrorResponse> UploadPlaylistImageAsync(string userId, string playlistId, string base64EncodedJpgImage)

        #endregion
    }
}
